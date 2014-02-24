using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RichEnumsExample.TestableCode
{
    // The class is developed here so it can be easily tested.  It is then manually copied into the T4 file.
    // This file itself is not needed for EnumGenerator.t4 to work.

    public sealed class RichEnumDescription
    {
        public sealed class CsvParseException : Exception
        {
            public CsvParseException( string message ) : base( message )
            {

            }
        }

        public sealed class Entry : IEnumerable<Field>
        {
            public readonly string Name;
            public readonly string Description;
            private readonly IEnumerable<Field> _fields;

            public Entry( string name, string description, IEnumerable<Field> fields )
            {
                Name = name;
                Description = description;
                _fields = fields;
            }

            public IEnumerator<Field> GetEnumerator()
            {
                return _fields.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public sealed class Field
        {
            public readonly FieldType Metadata;
            public readonly string Value;

            public Field( FieldType metadata, string value )
            {
                Metadata = metadata;
                Value = value;
            }
        }

        public sealed class FieldType
        {
            public readonly string RawName;
            public readonly Type Type;

            public FieldType( string rawName, Type type )
            {
                RawName = rawName;
                Type = type;
            }

            public string FieldName
            {
                get { return "_" + ArgumentName; }
            }

            public string ArgumentName
            {
                get { return Char.ToLowerInvariant( RawName[0] ) + RawName.Substring( 1 ); }
            }

            public string PropertyName
            {
                get { return Char.ToUpperInvariant( RawName[0] ) + RawName.Substring( 1 ); }
            }
        }

        public readonly string Description;
        private readonly List<string[]> _entryRows;

        public readonly IEnumerable<FieldType> EntryFields;

        public RichEnumDescription( string description, List<string[]> entryEntryRows )
        {
            Description = description ?? String.Empty;
            _entryRows = entryEntryRows;

            EntryFields = Enumerable.Empty<FieldType>();
        }

        public IEnumerable<Entry> GetEntries()
        {
            return
                _entryRows.Select(
                    row => new Entry(
                        name: row.First(),
                        description: row.Skip( 1 ).First(),
                        fields: Enumerable.Empty<Field>() ) );
        }

        #region Parsing CSV file

        private enum Stage
        {
            Names,
            Descriptions,
            Types,
            Entries,
        }

        public static RichEnumDescription ParseFile( IEnumerable<string> lines )
        {
            var currentStage = Stage.Names;

            string[] fieldNames = null;
            string enumDescription = null;
            var rows = new List<string[]>();

            foreach( var line in lines )
            {
                var rawRow = ParseRow( line ).ToArray();
                switch( currentStage )
                {
                    case Stage.Names:
                        ValidateNameRow( rawRow );
                        fieldNames = rawRow.Skip( 2 ).ToArray();
                        currentStage = Stage.Descriptions;
                        break;

                    case Stage.Descriptions:
                        ValidateDescriptionRow( rawRow, numFields: fieldNames.Length );
                        enumDescription = rawRow.ElementAt( 1 );
                        currentStage = fieldNames.Length > 0 ? Stage.Types : Stage.Entries;
                        break;

                    case Stage.Types:
                        ValidateTypeRow( rawRow, numFields: fieldNames.Length );
                        // Do type stuff here
                        currentStage = Stage.Entries;
                        break;

                    case Stage.Entries:
                        ValidateRowLength( rawRow, numFields: fieldNames.Length );
                        rows.Add( rawRow );
                        break;

                    default:
                        throw new CsvParseException( "Messed up parsing" );
                }
            }

            if( currentStage != Stage.Entries )
            {
                throw new CsvParseException( "Unexpected end of file." );
            }

            return new RichEnumDescription( enumDescription, rows );
        }

        private static void ValidateNameRow( string[] row )
        {
            if( row.Length < 2 )
            {
                throw new CsvParseException( "Name row is too short." );
            }
            if( row[0] != "Name" )
            {
                throw new CsvParseException(
                    String.Format( "Expected 'Name' in first column, got: {0}", row[0] ) );
            }
            if( row[1] != "Description" )
            {
                throw new CsvParseException(
                    String.Format( "Expected 'Description' in second column, got: {0}", row[1] ) );
            }
        }

        private static void ValidateDescriptionRow( string[] row, int numFields )
        {
            if( row[0] != "Descriptions" )
            {
                throw new CsvParseException(
                    String.Format( "Expected 'Descriptions' in first column, got: {0}", row[0] ) );
            }
            if( row.Length < ( 2 + numFields ) )
            {
                throw new CsvParseException( "Description row does not have enough columns." );
            }
        }

        private static void ValidateTypeRow( string[] row, int numFields )
        {
            if( row[0] != "Types" )
            {
                throw new CsvParseException(
                    String.Format( "Expected 'Types' in first column, got: {0}", row[0] ) );
            }
            if( row.Length < ( 2 + numFields ) )
            {
                throw new CsvParseException( "Type row does not have enough columns." );
            }
        }

        private static void ValidateRowLength( string[] row, int numFields )
        {
            if( row.Length < ( 2 + numFields ) )
            {
                throw new CsvParseException( "Row does not have enough columns." );
            }
        }

        #endregion

        #region Parsing CSV row

        private enum RowState
        {
            ColumnStart,
            UnescapedColumn,
            EscapedColumn,
            QuoteInEscapedColumn,
        }

        public static IEnumerable<string> ParseRow( string line )
        {
            var state = RowState.ColumnStart;

            var currentColumn = new StringBuilder();

            foreach( var c in line )
            {
                switch( state )
                {
                    case RowState.ColumnStart:
                        if( c == '"' )
                        {
                            state = RowState.EscapedColumn;
                        }
                        else if( c == ',' )
                        {
                            yield return currentColumn.ToString();
                            currentColumn.Clear();
                        }
                        else
                        {
                            state = RowState.UnescapedColumn;
                            currentColumn.Append( c );
                        }
                        break;

                    case RowState.UnescapedColumn:
                        if( c == '"' )
                        {
                            throw new ArgumentException( "Cannot have quote in unescaped column" );
                        }
                        else if( c == ',' )
                        {
                            yield return currentColumn.ToString();
                            currentColumn.Clear();
                            state = RowState.ColumnStart;
                        }
                        else
                        {
                            currentColumn.Append( c );
                        }
                        break;

                    case RowState.EscapedColumn:
                        if( c == '"' )
                        {
                            state = RowState.QuoteInEscapedColumn;
                        }
                        else
                        {
                            currentColumn.Append( c );
                        }
                        break;

                    case RowState.QuoteInEscapedColumn:
                        if( c == '"' )
                        {
                            currentColumn.Append( c );
                            state = RowState.EscapedColumn;
                        }
                        else if( c == ',' )
                        {
                            yield return currentColumn.ToString();
                            currentColumn.Clear();
                            state = RowState.ColumnStart;
                        }
                        else
                        {
                            throw new ArgumentException( "Invalid escaped column" );
                        }
                        break;
                }
            }

            if( currentColumn.Length > 0 )
            {
                yield return currentColumn.ToString();
            }
        }

        #endregion
    }
}
