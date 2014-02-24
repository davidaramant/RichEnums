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

        public readonly string Name;
        public readonly string Description;
        private readonly List<string[]> _rows;

        public readonly IEnumerable<FieldType> FieldTypes;

        public RichEnumDescription( string name, string description, string[] fieldNames, Type[] fieldTypes, List<string[]> rows )
        {
            Name = name;
            Description = description;
            //_rows = rows;

            //FieldTypes = fieldNames.Zip(
            //    fieldTypes,
            //    ( fieldName, fieldType ) => new FieldType( fieldName, fieldType ) ).ToArray();
        }

        public IEnumerable<Entry> GetEntries()
        {
            return
                null;
                //_rows.Select(
                //    row => new Entry(
                //        row.First(),
                //        FieldTypes.Zip(
                //            row.Skip( 1 ),
                //            ( fieldType, colValue ) => new Field( fieldType, colValue ) ) ) );
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

            int numFields = 0;
            string[] names = null;
            string enumName = null;
            string enumDescription = null;
            Type[] types = null;
            var rows = new List<string[]>();

            foreach( var line in lines )
            {
                switch( currentStage )
                {
                    case Stage.Names:
                        names = ParseRow( line ).Skip( 2 ).ToArray();
                        numFields = names.Length;
                        currentStage = Stage.Types;
                        break;

                    case Stage.Types:
                        var rawRow = ParseRow( line );
                        enumName = rawRow.ElementAt( 0 );
                        enumDescription = rawRow.ElementAt( 1 );
                        //types = rawRow.Skip( 2 ).Select( Type.GetType ).ToArray();
                        //if( types.Length != numFields )
                        //{
                        //    throw new ArgumentException( "Bad number of types." );
                        //}
                        currentStage = Stage.Entries;
                        break;

                    //case Stage.Entries:
                    //    var newRow = ParseRow( line ).ToArray();
                    //    if( newRow.Length != numFields + 1 ) // These rows include the name as well
                    //    {
                    //        throw new ArgumentException( "Bad number of columns in data row." );
                    //    }
                    //    rows.Add( newRow );
                    //    break;

                    //default:
                    //    throw new Exception( "Messed up parsing" );
                }
            }

            return new RichEnumDescription( enumName, enumDescription, names, types, rows );
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
