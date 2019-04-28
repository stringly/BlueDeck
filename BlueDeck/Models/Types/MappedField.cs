using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// Class used to map Table/Cell index and value
    /// </summary>
    public class MappedField
    {
        /// <summary>
        /// Gets or sets the index of the table.
        /// </summary>
        /// <remarks>
        /// This is the index of the table in the Template document
        /// </remarks>
        /// <value>
        /// The index of the table.
        /// </value>
        public int TableIndex { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <remarks>
        /// This property may replace the TableIndex completely, as the Table's index in the Document is 
        /// dependant on the part of the document that it is in, which means tables in the header/footer
        /// will be indexed according to the Header/Footer's tables collection, which is independent of the MainDocumentPart's
        /// table indexes.
        /// </remarks>
        /// <value>
        /// The table.
        /// </value>
        public Table Table { get; set; }

        /// <summary>
        /// Gets or sets the index of the row.
        /// </summary>
        /// <value>
        /// The index of the row.
        /// </value>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the cell.
        /// </summary>
        /// <value>
        /// The index of the cell.
        /// </value>
        public int CellIndex { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        public int Value { get; set; }

        /// <summary>
        /// Writes the data in the mapped field to the document.
        /// </summary>
        /// <param name="table">
        /// The target <see cref="T:DocumentFormat.OpenXml.Wordprocessing.Table"/>. 
        /// This parameter is required because the Table index depends on whether the MappedField is in a Header/Footer or in the document's mainpart, which means that the TableIndex is not
        /// sufficient to target the proper table. 
        /// </param>
        /// <param name="newText">The new text.</param>
        public void Write(string newText = null)
        {
            TableRow row = Table.Elements<TableRow>().ElementAt(RowIndex);
            TableCell cell = row.Elements<TableCell>().ElementAt(CellIndex);
            Paragraph p = cell.Elements<Paragraph>().First();
            Run r = new Run();
            RunProperties runProperties1 = new RunProperties();
            r.Append(runProperties1);
            if (newText != null)
            {
                Text t = new Text(newText);
                r.Append(t);
            }
            else
            {
                Text t = new Text(Value.ToString());
                r.Append(t);
            }
            p.Append(r);
        }
    }
}
