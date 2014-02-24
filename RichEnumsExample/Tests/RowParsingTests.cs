using System;
using System.Linq;
using NUnit.Framework;
using RichEnumsExample.TestableCode;

namespace Tests
{
    [TestFixture]
    public sealed class RowParsingTests
    {
        [Test]
        public void ShouldHandleEmptyRow()
        {
            Assert.That( RichEnumDescription.ParseRow( String.Empty ), Is.Empty, "Did not handle empty row." );
        }

        [Test]
        public void ShouldHandleJustSeparators()
        {
            Assert.That(
                RichEnumDescription.ParseRow( ",," ).ToArray(),
                Is.EqualTo( new[] { String.Empty, String.Empty } ),
                "Did not parse empty columns." );
        }

        [Test]
        public void ShouldHandleSingleUnescaptedColumn()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "Column" ).ToArray(),
                Is.EqualTo( new[] { "Column" } ),
                "Did not parse single column." );
        }

        [Test]
        public void ShouldHandleMultipleUnescapedColumns()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "Column1,Column2" ).ToArray(),
                Is.EqualTo( new[] { "Column1", "Column2" } ),
                "Did not parse multiple columns." );
        }

        [Test]
        public void ShouldHandleSingleEscaptedColumn()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "\"Column\"" ).ToArray(),
                Is.EqualTo( new[] { "Column" } ),
                "Did not parse single column." );
        }

        [Test]
        public void ShouldHandleMultipleEscapedColumns()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "\"Column1\",\"Column2\"" ).ToArray(),
                Is.EqualTo( new[] { "Column1", "Column2" } ),
                "Did not parse multiple columns." );
        }

        [Test]
        public void ShouldHandleSingleUnescaptedColumnWithSpaces()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "Column 1" ).ToArray(),
                Is.EqualTo( new[] { "Column 1" } ),
                "Did not parse single column." );
        }

        [Test]
        public void ShouldHandleMultipleUnescapedColumnsWithSpaces()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "Column 1,Column 2" ).ToArray(),
                Is.EqualTo( new[] { "Column 1", "Column 2" } ),
                "Did not parse multiple columns." );
        }

        [Test]
        public void ShouldHandleSingleEscaptedColumnWithSpaces()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "\"Column 1\"" ).ToArray(),
                Is.EqualTo( new[] { "Column 1" } ),
                "Did not parse single column." );
        }

        [Test]
        public void ShouldHandleMultipleEscapedColumnsWithSpaces()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "\"Column 1\",\"Column 2\"" ).ToArray(),
                Is.EqualTo( new[] { "Column 1", "Column 2" } ),
                "Did not parse multiple columns." );
        }

        [Test]
        public void ShouldHandleEscapedColumnsWithCommas()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "\"Column, Number 1\",\"Column, Number 2\"" ).ToArray(),
                Is.EqualTo( new[] { "Column, Number 1", "Column, Number 2" } ),
                "Did not parse multiple columns." );
        }


        [Test]
        public void ShouldHandleEscapedColumnsWithQuotes()
        {
            Assert.That(
                RichEnumDescription.ParseRow( "\"Column \"\"1\"\"\",\"Column \"\"2\"\"\"" ).ToArray(),
                Is.EqualTo( new[] { "Column \"1\"", "Column \"2\"" } ),
                "Did not parse multiple columns." );
        }
    }
}
