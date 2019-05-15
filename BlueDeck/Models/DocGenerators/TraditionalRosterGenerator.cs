using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BlueDeck.Models.Types;

// TODO: Rewire to use ChildComponent Collection of a single component
namespace BlueDeck.Models.DocGenerators
{
    public class TraditionalRosterGenerator
    {
        public List<Component> Components { get; set; }
        private int ComponentTableCount { get; set; }
        public TraditionalRosterGenerator()
        {
        }
        public TraditionalRosterGenerator(List<Component> _components)
        {
            Components = _components;
            ComponentTableCount = 0;
        }

        public MemoryStream Generate()
        {
            var mem = new MemoryStream();
            byte[] byteArray = File.ReadAllBytes("Templates/Traditional_Roster_Template_Columns.docx");
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(mem, true))
            {

                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                // populate the header
                SetTableHeaderContent(mainPart.HeaderParts.ElementAt(1).RootElement.Elements<Table>().ElementAt(0));
                // Demographics                
                SetDemographicTableContent(mainPart.FooterParts.ElementAt(0).RootElement.Elements<Table>().ElementAt(0));

                // Centered, Commander/Assistant Commander spot
                mainPart.Document.Body.Elements<Paragraph>().ElementAt(0).InsertBeforeSelf(GenerateCenteredSoloTable(Components.First().Positions.FirstOrDefault(x => x.IsManager == true)));


                // Append Supervisors from First Gen Children ONLY
                List<Position> firstGenSupervisors = new List<Position>();
                foreach (Component child in Components.First().ChildComponents)
                {
                    firstGenSupervisors.Add(child?.Positions?.Where(p => p.IsManager == true).FirstOrDefault());
                }
                foreach (Position supervisor in firstGenSupervisors)
                {
                    mainPart.Document.Body.Append(GenerateLeftJustifiedTable(supervisor));
                }
                // Break the Column, move the next box to the right column
                mainPart.Document.Body.Append(GenerateColumnBreakParagraph());

                // Append Main Component non-supervisory positions
                if (Components.First().Positions != null && Components.First().Positions.Count > 0)
                {
                    foreach (Position position in Components.First().Positions)
                    {
                        if (position.IsManager != true)
                        {
                            mainPart.Document.Body.Append(GenerateLeftJustifiedTable(position));
                        }
                    }
                }
                else
                {
                    RunProperties runProperties5 = new RunProperties();
                    RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS" };
                    FontSize fontSize5 = new FontSize() { Val = "18" };
                    runProperties5.Append(runFonts5);
                    runProperties5.Append(fontSize5);
                    Run run5 = new Run();
                    Text text5 = new Text("NONE");
                    run5.Append(runProperties5);
                    run5.Append(text5);
                    mainPart.Document.Body.Append(new Paragraph(run5));
                }
                // Break Section to move the next table to the left column
                mainPart.Document.Body.Append(GenerateSectionBreakParagraph());


                mainPart.Document.Body.Append(GenerateExceptionToDutyStatusTable(Components.FirstOrDefault().GetExceptionToDutyMembersRecursive()));

                // Break Section to move the next table to the left column
                mainPart.Document.Body.Append(GenerateSectionBreakParagraph());
                // Break the page, so subsequent components are on a different page
                //mainPart.Document.Body.Append(new Paragraph());
                mainPart.Document.Body.Append(GeneratePageBreakSectionProperties());

                // Render First-Gen Children
                foreach (Component c in Components.FirstOrDefault().ChildComponents)
                {
                    // Render the Manager's Box
                    mainPart.Document.Body.Append(GenerateFullWidthTable(c));
                    mainPart.Document.Body.Append(new Paragraph());
                    mainPart.Document.Body.Append(GenerateTwoColumnParagraph());
                    // spacer paragraph
                    RunProperties runProperties2 = new RunProperties();
                    RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS" };
                    FontSize fontSize2 = new FontSize() { Val = "18" };
                    runProperties2.Append(runFonts2);
                    runProperties2.Append(fontSize2);
                    Run run2 = new Run();
                    Text text2 = new Text("Immediate Positions:");
                    run2.Append(runProperties2);
                    run2.Append(text2);

                    mainPart.Document.Body.Append(new Paragraph(new Run(run2)));
                    // Append child's non-supervisory positions
                    if (c.Positions != null && c.Positions.Count > 0)
                    {
                        foreach (Position p in c.Positions)
                        {
                            if (p.IsManager != true)
                            {
                                mainPart.Document.Body.Append(GenerateLeftJustifiedTable(p));
                            }
                        }
                    }
                    else
                    {
                        RunProperties runProperties3 = new RunProperties();
                        RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS" };
                        FontSize fontSize3 = new FontSize() { Val = "18" };
                        runProperties3.Append(runFonts3);
                        runProperties3.Append(fontSize3);
                        Run run3 = new Run();
                        Text text3 = new Text("NONE");
                        run3.Append(runProperties3);
                        run3.Append(text3);
                        mainPart.Document.Body.Append(new Paragraph(run3));
                    }


                    // move to right column
                    Paragraph exceptionToDuty = GenerateColumnBreakParagraph();
                    RunProperties runProperties1 = new RunProperties();
                    RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS" };
                    FontSize fontSize1 = new FontSize() { Val = "18" };
                    runProperties1.Append(runFonts1);
                    runProperties1.Append(fontSize1);
                    Run run1 = new Run();
                    Text text1 = new Text("Exceptions to Normal Duty:");
                    run1.Append(runProperties1);
                    run1.Append(text1);
                    exceptionToDuty.Append(run1);
                    mainPart.Document.Body.Append(exceptionToDuty);
                    // render exception to duty
                    if (c.GetExceptionToDutyMembersRecursive().Count > 0)
                    {
                        mainPart.Document.Body.Append(GenerateExceptionToDutyStatusTable(c.GetExceptionToDutyMembersRecursive()));
                    }
                    else
                    {
                        RunProperties runProperties4 = new RunProperties();
                        RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS" };
                        FontSize fontSize4 = new FontSize() { Val = "18" };
                        runProperties4.Append(runFonts4);
                        runProperties4.Append(fontSize4);
                        Run run4 = new Run();
                        Text text4 = new Text("NONE");
                        run4.Append(runProperties4);
                        run4.Append(text4);
                        mainPart.Document.Body.Append(new Paragraph(run4));
                    }
                    // back to right column
                    mainPart.Document.Body.Append(GenerateSectionBreakParagraph());

                    // deal with children of first level children
                    if (c.ChildComponents != null && c.ChildComponents.Count > 0)
                    {
                        // reset the Odd/Even check
                        ComponentTableCount = 0;
                        foreach (Component child in c.ChildComponents)
                        {

                            RecursiveComponentTable(mainPart, child);
                        }
                    }


                    // Break the page, so subsequent components are on a different page
                    //mainPart.Document.Body.Append(new Paragraph());
                    mainPart.Document.Body.Append(GeneratePageBreakSectionProperties());
                }

                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }

        public void SetTableHeaderContent(Table headerTable)
        {
            RunProperties runProperties1 = new RunProperties();
            RunFonts runFont1 = new RunFonts { Ascii = "Trebuchet MS" };
            runProperties1.Append(runFont1);
            FontSize fontSize1 = new FontSize() { Val = "28" };
            runProperties1.Append(fontSize1);
            Run run1 = new Run();
            run1.Append(runProperties1);
            Text text1 = new Text($"{Components.First().Name.ToUpper()} PERSONNEL ROSTER");
            run1.Append(text1);

            headerTable.Elements<TableRow>().ElementAt(0)
                .Elements<TableCell>().ElementAt(1)
                    .Elements<Paragraph>().ElementAt(0)
                        .Append(run1);
            Run run2 = new Run();
            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS" };
            runProperties2.Append(runFonts2);
            run2.Append(runProperties2);
            Text text2 = new Text(DateTime.Now.ToString("MMMM dd, yyyy"));
            run2.Append(text2);
            headerTable.Elements<TableRow>().ElementAt(1)
                .Elements<TableCell>().ElementAt(1)
                    .Elements<Paragraph>().ElementAt(0)
                        .Append(run2);
        }
        public void SetDemographicTableContent(Table demoTable)
        {
            // Get a list of unique Rank values from the component's Members
            List<Rank> distinctRankList = Components.First().GetComponentMembersRecursive()
                .Select(x => x.Rank)
                .OrderByDescending(x => x.RankId)
                .Distinct()
                .OrderByDescending(x => x.IsSworn)
                .ToList();
            foreach (Rank distinctRank in distinctRankList)
            {
                List<Member> rankMembers = Components.First().GetComponentMembersRecursive().Where(x => x.Rank == distinctRank).ToList();
                demoTable.Append(GenerateDemographicTableRow(distinctRank.RankFullName, rankMembers));
            }
            TableCell summaryCell = demoTable.Elements<TableRow>().ElementAt(0).Elements<TableCell>().ElementAt(5);
            // Append "Sworn" totals row
            List<Member> swornMembers = Components.First().GetComponentMembersRecursive().Where(x => x.Rank.IsSworn == true).ToList();
            demoTable.Append(GenerateDemographicTableRow("Total Sworn", swornMembers, true));
            Run run3 = new Run();
            Text text3 = new Text($"Sworn Personnel: {swornMembers.Count}");
            run3.Append(text3);
            summaryCell.Elements<Paragraph>().First().Append(run3);
            // Append "Civilian" totals row
            List<Member> civMembers = Components.First().GetComponentMembersRecursive().Where(x => x.Rank.IsSworn == false).ToList();
            demoTable.Append(GenerateDemographicTableRow("Total Civilian", civMembers, false));
            Paragraph paragraph4 = new Paragraph();
            Run run4 = new Run();
            Text text4 = new Text($"Civilian Personnel: {civMembers.Count}");
            run4.Append(text4);
            paragraph4.Append(run4);
            summaryCell.Append(paragraph4);

            // finally, append "Station Totals" row
            demoTable.Append(GenerateDemographicTableRow("Total", Components.First().GetComponentMembersRecursive(), true));
            Paragraph paragraph5 = new Paragraph();
            Run run5 = new Run();
            RunProperties runProperties5 = new RunProperties();
            Bold bold5 = new Bold();
            runProperties5.Append(bold5);
            run5.Append(runProperties5);
            Text text5 = new Text($"Total Strength: {Components.First().GetComponentMembersRecursive().Count}");
            run5.Append(text5);
            paragraph5.Append(run5);
            summaryCell.Append(paragraph5);
        }
        public void RecursiveComponentTable(MainDocumentPart mainPart, Component c)
        {
            if (ComponentTableCount % 2 == 0)
            {
                mainPart.Document.Body.Append(new Paragraph());
            }
            //else
            //{
            mainPart.Document.Body.Append(GenerateComponentTable(c));
            //}                
            
            // increment the count
            ComponentTableCount++;

            // append appropriate break depending on odd/event count
            if (ComponentTableCount % 2 != 0)
            {
                mainPart.Document.Body.Append(GenerateColumnBreakParagraph());
            }
            else
            {
                mainPart.Document.Body.Append(GenerateSectionBreakParagraph());
            }
            if (c.ChildComponents != null && c.ChildComponents.Count > 0)
            {
                foreach (Component child in c.ChildComponents)
                {
                    RecursiveComponentTable(mainPart, child);
                }
            }
        }
        public Table GenerateCenteredSoloTable(Position _p)
        {
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "7200", Type = TableWidthUnitValues.Dxa };
            TableJustification tableJustification1 = new TableJustification() { Val = TableRowAlignmentValues.Center };
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableJustification1);
            tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "1440" };
            GridColumn gridColumn2 = new GridColumn() { Width = "3510" };
            GridColumn gridColumn3 = new GridColumn() { Width = "720" };
            GridColumn gridColumn4 = new GridColumn() { Width = "720" };
            GridColumn gridColumn5 = new GridColumn() { Width = "450" };
            GridColumn gridColumn6 = new GridColumn() { Width = "360" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00D7521E", RsidTableRowProperties = "006C4777" };

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableJustification tableJustification2 = new TableJustification() { Val = TableRowAlignmentValues.Center };

            tableRowProperties1.Append(tableJustification2);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4950", Type = TableWidthUnitValues.Dxa };
            GridSpan gridSpan1 = new GridSpan() { Val = 2 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(gridSpan1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00E26227", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold1 = new Bold();
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold2 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();

            // SET POSITION NAME
            text1.Text = _p.Name;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(bookmarkStart1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();

            // STATIC, ID number column header
            text2.Text = "ID";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();

            // STATIC; Callsign column Header
            text3.Text = "CALL";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders1);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();

            // STATIC; Race Header
            text4.Text = "R";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders2);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();

            // STATIC - Gender Column Header
            text5.Text = "S";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);

            TableRow tableRow2 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00D7521E", RsidTableRowProperties = "006C4777" };

            TableRowProperties tableRowProperties2 = new TableRowProperties();
            TableJustification tableJustification3 = new TableJustification() { Val = TableRowAlignmentValues.Center };

            tableRowProperties2.Append(tableJustification3);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1440", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();

            // SET Member Rank Value
            text6.Text = _p?.Members?.FirstOrDefault()?.Rank.RankFullName ?? "N/A";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "3510", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(tableCellVerticalAlignment7);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00365EA5" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties7.Append(runFonts13);
            paragraphMarkRunProperties7.Append(fontSize13);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript13);

            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize14 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts14);
            runProperties7.Append(fontSize14);
            runProperties7.Append(fontSizeComplexScript14);
            Text text7 = new Text();

            // SET Member Name LAST, First
            text7.Text = _p?.Members?.FirstOrDefault().GetLastNameFirstName() ?? "N/A";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run7);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(tableCellVerticalAlignment8);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00365EA5" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize15 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties8.Append(runFonts15);
            paragraphMarkRunProperties8.Append(fontSize15);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript15);

            paragraphProperties8.Append(justification5);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize16 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "18" };

            runProperties8.Append(runFonts16);
            runProperties8.Append(fontSize16);
            runProperties8.Append(fontSizeComplexScript16);
            Text text8 = new Text();

            // SET Member Badge Number
            text8.Text = _p?.Members?.FirstOrDefault().IdNumber ?? "N/A";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run8);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellVerticalAlignment9);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00365EA5" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize17 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties9.Append(runFonts17);
            paragraphMarkRunProperties9.Append(fontSize17);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript17);

            paragraphProperties9.Append(justification6);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run9 = new Run();

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize18 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "18" };

            runProperties9.Append(runFonts18);
            runProperties9.Append(fontSize18);
            runProperties9.Append(fontSizeComplexScript18);
            Text text9 = new Text();

            // SET Position Callsign
            text9.Text = _p?.Callsign?.ToUpper() ?? "NONE";

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(rightBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment10 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellBorders3);
            tableCellProperties10.Append(tableCellVerticalAlignment10);

            Paragraph paragraph10 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00A606A4" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize19 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties10.Append(runFonts19);
            paragraphMarkRunProperties10.Append(fontSize19);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript19);

            paragraphProperties10.Append(justification7);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run10 = new Run();

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize20 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "18" };

            runProperties10.Append(runFonts20);
            runProperties10.Append(fontSize20);
            runProperties10.Append(fontSizeComplexScript20);
            Text text10 = new Text();

            // SET Member Race Abbreviation
            text10.Text = _p?.Members?.FirstOrDefault().Race?.Abbreviation.ToString() ?? "-";

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run10);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(leftBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment11 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties11.Append(tableCellWidth11);
            tableCellProperties11.Append(tableCellBorders4);
            tableCellProperties11.Append(tableCellVerticalAlignment11);

            Paragraph paragraph11 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize21 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties11.Append(runFonts21);
            paragraphMarkRunProperties11.Append(fontSize21);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript21);

            paragraphProperties11.Append(justification8);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run11 = new Run();

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts22 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize22 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript() { Val = "18" };

            runProperties11.Append(runFonts22);
            runProperties11.Append(fontSize22);
            runProperties11.Append(fontSizeComplexScript22);
            Text text11 = new Text();

            // SET Gender Abbreviation
            text11.Text = _p?.Members?.FirstOrDefault()?.Gender.Abbreviation.ToString() ?? "-";

            run11.Append(runProperties11);
            run11.Append(text11);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run11);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            tableRow2.Append(tableRowProperties2);
            tableRow2.Append(tableCell6);
            tableRow2.Append(tableCell7);
            tableRow2.Append(tableCell8);
            tableRow2.Append(tableCell9);
            tableRow2.Append(tableCell10);
            tableRow2.Append(tableCell11);
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            table1.Append(bookmarkEnd1);
            return table1;
        }
        public Table GenerateLeftJustifiedTable(Position _p)
        {
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "7038", Type = TableWidthUnitValues.Dxa };
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "1183" };
            GridColumn gridColumn2 = new GridColumn() { Width = "2249" };
            GridColumn gridColumn3 = new GridColumn() { Width = "612" };
            GridColumn gridColumn4 = new GridColumn() { Width = "832" };
            GridColumn gridColumn5 = new GridColumn() { Width = "380" };
            GridColumn gridColumn6 = new GridColumn() { Width = "355" };
            //GridColumn gridColumn7 = new GridColumn(){ Width = "1427" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);
            //tableGrid1.Append(gridColumn7);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00D7521E", RsidTableRowProperties = "00BB2FD0" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "3432", Type = TableWidthUnitValues.Dxa };
            GridSpan gridSpan1 = new GridSpan() { Val = 2 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(gridSpan1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00E26227", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold1 = new Bold();
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold2 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = _p?.Name;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "612", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = "ID";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "832", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = "CALL";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "380", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders1);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = "R";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "355", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders2);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = "S";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1427", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D7521E", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D7521E" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(paragraphMarkRunProperties6);

            paragraph6.Append(paragraphProperties6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            //tableRow1.Append(tableCell6);

            TableRow tableRow2 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00D317D3", RsidTableRowProperties = "00E369F0" };

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "1183", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(tableCellVerticalAlignment7);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties7.Append(runFonts12);
            paragraphMarkRunProperties7.Append(fontSize12);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript12);

            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts13);
            runProperties6.Append(fontSize13);
            runProperties6.Append(fontSizeComplexScript13);
            Text text6 = new Text();
            text6.Text = _p?.Members?.FirstOrDefault()?.Rank?.RankShort ?? "N/A";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run6);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "2249", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(tableCellVerticalAlignment8);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize14 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties8.Append(runFonts14);
            paragraphMarkRunProperties8.Append(fontSize14);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript14);

            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize15 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts15);
            runProperties7.Append(fontSize15);
            runProperties7.Append(fontSizeComplexScript15);
            Text text7 = new Text();
            text7.Text = _p?.Members?.FirstOrDefault()?.GetLastNameFirstName() ?? "VACANT";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run7);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "612", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellVerticalAlignment9);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize16 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties9.Append(runFonts16);
            paragraphMarkRunProperties9.Append(fontSize16);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript16);

            paragraphProperties9.Append(justification5);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize17 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "18" };

            runProperties8.Append(runFonts17);
            runProperties8.Append(fontSize17);
            runProperties8.Append(fontSizeComplexScript17);
            Text text8 = new Text();
            text8.Text = _p?.Members?.FirstOrDefault()?.IdNumber ?? "N/A";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run8);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "832", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment10 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellVerticalAlignment10);

            Paragraph paragraph10 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize18 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties10.Append(runFonts18);
            paragraphMarkRunProperties10.Append(fontSize18);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript18);

            paragraphProperties10.Append(justification6);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run9 = new Run();

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize19 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "18" };

            runProperties9.Append(runFonts19);
            runProperties9.Append(fontSize19);
            runProperties9.Append(fontSizeComplexScript19);
            Text text9 = new Text();
            text9.Text = _p?.Callsign ?? "NONE";

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run9);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "380", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(rightBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment11 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties11.Append(tableCellWidth11);
            tableCellProperties11.Append(tableCellBorders3);
            tableCellProperties11.Append(tableCellVerticalAlignment11);

            Paragraph paragraph11 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize20 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties11.Append(runFonts20);
            paragraphMarkRunProperties11.Append(fontSize20);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript20);

            paragraphProperties11.Append(justification7);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run10 = new Run();

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize21 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "18" };

            runProperties10.Append(runFonts21);
            runProperties10.Append(fontSize21);
            runProperties10.Append(fontSizeComplexScript21);
            Text text10 = new Text();
            text10.Text = _p?.Members?.FirstOrDefault()?.Race?.Abbreviation.ToString() ?? "-";

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run10);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            TableCell tableCell12 = new TableCell();

            TableCellProperties tableCellProperties12 = new TableCellProperties();
            TableCellWidth tableCellWidth12 = new TableCellWidth() { Width = "355", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(leftBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment12 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties12.Append(tableCellWidth12);
            tableCellProperties12.Append(tableCellBorders4);
            tableCellProperties12.Append(tableCellVerticalAlignment12);

            Paragraph paragraph12 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties12 = new ParagraphProperties();
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties12 = new ParagraphMarkRunProperties();
            RunFonts runFonts22 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize22 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties12.Append(runFonts22);
            paragraphMarkRunProperties12.Append(fontSize22);
            paragraphMarkRunProperties12.Append(fontSizeComplexScript22);

            paragraphProperties12.Append(justification8);
            paragraphProperties12.Append(paragraphMarkRunProperties12);

            Run run11 = new Run();

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts23 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize23 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript23 = new FontSizeComplexScript() { Val = "18" };

            runProperties11.Append(runFonts23);
            runProperties11.Append(fontSize23);
            runProperties11.Append(fontSizeComplexScript23);
            Text text11 = new Text();
            text11.Text = _p?.Members?.FirstOrDefault()?.Gender.Abbreviation.ToString() ?? "-";

            run11.Append(runProperties11);
            run11.Append(text11);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            paragraph12.Append(paragraphProperties12);
            paragraph12.Append(run11);
            paragraph12.Append(bookmarkStart1);
            paragraph12.Append(bookmarkEnd1);

            tableCell12.Append(tableCellProperties12);
            tableCell12.Append(paragraph12);

            TableCell tableCell13 = new TableCell();

            TableCellProperties tableCellProperties13 = new TableCellProperties();
            TableCellWidth tableCellWidth13 = new TableCellWidth() { Width = "1427", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment13 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties13.Append(tableCellWidth13);
            tableCellProperties13.Append(tableCellVerticalAlignment13);

            Paragraph paragraph13 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties13 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties13 = new ParagraphMarkRunProperties();
            RunFonts runFonts24 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize24 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript24 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties13.Append(runFonts24);
            paragraphMarkRunProperties13.Append(fontSize24);
            paragraphMarkRunProperties13.Append(fontSizeComplexScript24);

            paragraphProperties13.Append(paragraphMarkRunProperties13);

            paragraph13.Append(paragraphProperties13);

            tableCell13.Append(tableCellProperties13);
            tableCell13.Append(paragraph13);

            tableRow2.Append(tableCell7);
            tableRow2.Append(tableCell8);
            tableRow2.Append(tableCell9);
            tableRow2.Append(tableCell10);
            tableRow2.Append(tableCell11);
            tableRow2.Append(tableCell12);
            //tableRow2.Append(tableCell13);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            return table1;
        }
        public Table GenerateFloatRightTable(string _title, List<Member> members)
        {
            // Grab the first member from the list... I have to add the first row
            // when the table is generated or the formatting will be fucky
            Member firstMember = members.FirstOrDefault();
            // remove the first member, so we don't add it twice when we add the additional rows.
            members.Remove(firstMember);
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TablePositionProperties tablePositionProperties1 = new TablePositionProperties() { LeftFromText = 180, RightFromText = 180, VerticalAnchor = VerticalAnchorValues.Page, HorizontalAnchor = HorizontalAnchorValues.Margin, TablePositionXAlignment = HorizontalAlignmentValues.Right, TablePositionY = 3500 };
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "0000", FirstRow = false, LastRow = false, FirstColumn = false, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = false };

            tableProperties1.Append(tablePositionProperties1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "1165" };
            GridColumn gridColumn2 = new GridColumn() { Width = "2520" };
            GridColumn gridColumn3 = new GridColumn() { Width = "900" };
            GridColumn gridColumn4 = new GridColumn() { Width = "1170" };
            GridColumn gridColumn5 = new GridColumn() { Width = "360" };
            GridColumn gridColumn6 = new GridColumn() { Width = "360" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);

            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "007F3139", RsidTableRowProperties = "007F3139", ParagraphId = "3356D3A2", TextId = "77777777" };

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)167U };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "3685", Type = TableWidthUnitValues.Dxa };
            GridSpan gridSpan1 = new GridSpan() { Val = 2 };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(gridSpan1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "001D57ED", RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "01959A68", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0" };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold1 = new Bold();
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold2 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = _title;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "900", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "63513317", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "0" };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(spacingBetweenLines2);
            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = "ID";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1170", Type = TableWidthUnitValues.Dxa };

            tableCellProperties3.Append(tableCellWidth3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "3D21D2BF", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0" };
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(spacingBetweenLines3);
            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = "CALL";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellVerticalAlignment1);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "559EC313", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0" };
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(spacingBetweenLines4);
            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = "R";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            tableCellProperties5.Append(tableCellWidth5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "46AE9A00", TextId = "77777777" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0" };
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(spacingBetweenLines5);
            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = "S";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);

            TableRow tableRow2 = new TableRow() { RsidTableRowAddition = "007F3139", RsidTableRowProperties = "007F3139", ParagraphId = "31D47F0C", TextId = "77777777" };

            TableRowProperties tableRowProperties2 = new TableRowProperties();
            TableRowHeight tableRowHeight2 = new TableRowHeight() { Val = (UInt32Value)140U };

            tableRowProperties2.Append(tableRowHeight2);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1165", Type = TableWidthUnitValues.Dxa };

            tableCellProperties6.Append(tableCellWidth6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "0008380A", RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "2501BB54", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines() { After = "0" };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(spacingBetweenLines6);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();
            text6.Text = firstMember?.Rank.RankShort ?? "N/A";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "2520", Type = TableWidthUnitValues.Dxa };

            tableCellProperties7.Append(tableCellWidth7);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "0008380A", RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "4E7C5850", TextId = "77777777" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines7 = new SpacingBetweenLines() { After = "0" };

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties7.Append(runFonts13);
            paragraphMarkRunProperties7.Append(fontSize13);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript13);

            paragraphProperties7.Append(spacingBetweenLines7);
            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize14 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts14);
            runProperties7.Append(fontSize14);
            runProperties7.Append(fontSizeComplexScript14);
            Text text7 = new Text();
            text7.Text = firstMember?.GetLastNameFirstName() ?? "N/A";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run7);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "900", Type = TableWidthUnitValues.Dxa };

            tableCellProperties8.Append(tableCellWidth8);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "1354C3E4", TextId = "77777777" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines8 = new SpacingBetweenLines() { After = "0" };
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize15 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties8.Append(runFonts15);
            paragraphMarkRunProperties8.Append(fontSize15);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript15);

            paragraphProperties8.Append(spacingBetweenLines8);
            paragraphProperties8.Append(justification5);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize16 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "18" };

            runProperties8.Append(runFonts16);
            runProperties8.Append(fontSize16);
            runProperties8.Append(fontSizeComplexScript16);
            Text text8 = new Text();
            text8.Text = firstMember?.IdNumber ?? "N/A";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run8);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "1170", Type = TableWidthUnitValues.Dxa };

            tableCellProperties9.Append(tableCellWidth9);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "35EBB89A", TextId = "77777777" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines9 = new SpacingBetweenLines() { After = "0" };
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize17 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties9.Append(runFonts17);
            paragraphMarkRunProperties9.Append(fontSize17);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript17);

            paragraphProperties9.Append(spacingBetweenLines9);
            paragraphProperties9.Append(justification6);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run9 = new Run();

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize18 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "18" };

            runProperties9.Append(runFonts18);
            runProperties9.Append(fontSize18);
            runProperties9.Append(fontSizeComplexScript18);
            Text text9 = new Text();
            text9.Text = firstMember?.Position?.Callsign ?? "NONE";

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellVerticalAlignment2);

            Paragraph paragraph10 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "2F15BD52", TextId = "77777777" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines() { After = "0" };
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize19 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties10.Append(runFonts19);
            paragraphMarkRunProperties10.Append(fontSize19);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript19);

            paragraphProperties10.Append(spacingBetweenLines10);
            paragraphProperties10.Append(justification7);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run10 = new Run();

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize20 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "18" };

            runProperties10.Append(runFonts20);
            runProperties10.Append(fontSize20);
            runProperties10.Append(fontSizeComplexScript20);
            Text text10 = new Text();
            text10.Text = firstMember?.Race.Abbreviation.ToString() ?? "-";

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run10);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            tableCellProperties11.Append(tableCellWidth11);

            Paragraph paragraph11 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "700CB04D", TextId = "77777777" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines11 = new SpacingBetweenLines() { After = "0" };
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize21 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties11.Append(runFonts21);
            paragraphMarkRunProperties11.Append(fontSize21);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript21);

            paragraphProperties11.Append(spacingBetweenLines11);
            paragraphProperties11.Append(justification8);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run11 = new Run();

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts22 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize22 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript() { Val = "18" };

            runProperties11.Append(runFonts22);
            runProperties11.Append(fontSize22);
            runProperties11.Append(fontSizeComplexScript22);
            Text text11 = new Text();
            text11.Text = firstMember?.Gender.Abbreviation.ToString() ?? "-";

            run11.Append(runProperties11);
            run11.Append(text11);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run11);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            tableRow2.Append(tableRowProperties2);
            tableRow2.Append(tableCell6);
            tableRow2.Append(tableCell7);
            tableRow2.Append(tableCell8);
            tableRow2.Append(tableCell9);
            tableRow2.Append(tableCell10);
            tableRow2.Append(tableCell11);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);

            foreach (Member m in members)
            {
                table1.Append(GenerateFloatRightTableRow(m));
            }

            return table1;

        }
        public Table GenerateExceptionToDutyStatusTable(List<Member> _members)
        {
            Member firstMember = _members.First();
            _members.Remove(firstMember);
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "7105", Type = TableWidthUnitValues.Dxa };
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "985" };
            GridColumn gridColumn2 = new GridColumn() { Width = "2610" };
            GridColumn gridColumn3 = new GridColumn() { Width = "720" };
            GridColumn gridColumn4 = new GridColumn() { Width = "450" };
            GridColumn gridColumn5 = new GridColumn() { Width = "360" };
            GridColumn gridColumn6 = new GridColumn() { Width = "1980" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00FA49F6", RsidTableRowProperties = "00FA49F6", ParagraphId = "483B08E0", TextId = "2C04F0DF" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "3595", Type = TableWidthUnitValues.Dxa };
            GridSpan gridSpan1 = new GridSpan() { Val = 2 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(gridSpan1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00E26227", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "422C1E80", TextId = "1D3CE084" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold1 = new Bold();
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold2 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = "Exception to Duty";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "319AE63F", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = "ID";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders1);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "2AA43DFE", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = "R";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders2);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "79EB0566", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = "S";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "1980", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(leftBorder2);

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders3);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "549965F0", TextId = "1FFF21E7" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = "Status";

            run5.Append(runProperties5);
            run5.Append(text5);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);
            paragraph5.Append(bookmarkStart1);
            paragraph5.Append(bookmarkEnd1);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);

            TableRow tableRow2 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00FA49F6", RsidTableRowProperties = "00FA49F6", ParagraphId = "2BC69CB0", TextId = "1BE89A9B" };

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "985", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment5);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "1BD045BB", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();
            text6.Text = firstMember?.Rank.RankShort ?? "N/A";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "2610", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(tableCellVerticalAlignment6);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "200FCDEF", TextId = "77777777" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties7.Append(runFonts13);
            paragraphMarkRunProperties7.Append(fontSize13);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript13);

            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize14 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts14);
            runProperties7.Append(fontSize14);
            runProperties7.Append(fontSizeComplexScript14);
            Text text7 = new Text();
            text7.Text = firstMember?.GetLastNameFirstName() ?? "N/A";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run7);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(tableCellVerticalAlignment7);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "195C09BE", TextId = "77777777" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize15 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties8.Append(runFonts15);
            paragraphMarkRunProperties8.Append(fontSize15);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript15);

            paragraphProperties8.Append(justification5);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize16 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "18" };

            runProperties8.Append(runFonts16);
            runProperties8.Append(fontSize16);
            runProperties8.Append(fontSizeComplexScript16);
            Text text8 = new Text();
            text8.Text = firstMember?.IdNumber ?? "N/A";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run8);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(rightBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellBorders4);
            tableCellProperties9.Append(tableCellVerticalAlignment8);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "282BF12F", TextId = "77777777" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize17 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties9.Append(runFonts17);
            paragraphMarkRunProperties9.Append(fontSize17);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript17);

            paragraphProperties9.Append(justification6);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run9 = new Run();

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize18 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "18" };

            runProperties9.Append(runFonts18);
            runProperties9.Append(fontSize18);
            runProperties9.Append(fontSizeComplexScript18);
            Text text9 = new Text();
            text9.Text = firstMember?.Race.Abbreviation.ToString() ?? "-";

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders5.Append(leftBorder3);
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellBorders5);
            tableCellProperties10.Append(tableCellVerticalAlignment9);

            Paragraph paragraph10 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "38D057D7", TextId = "77777777" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize19 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties10.Append(runFonts19);
            paragraphMarkRunProperties10.Append(fontSize19);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript19);

            paragraphProperties10.Append(justification7);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run10 = new Run();

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize20 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "18" };

            runProperties10.Append(runFonts20);
            runProperties10.Append(fontSize20);
            runProperties10.Append(fontSizeComplexScript20);
            Text text10 = new Text();
            text10.Text = firstMember?.Gender.Abbreviation.ToString() ?? "-";

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run10);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "1980", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders6 = new TableCellBorders();
            LeftBorder leftBorder4 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders6.Append(leftBorder4);

            tableCellProperties11.Append(tableCellWidth11);
            tableCellProperties11.Append(tableCellBorders6);

            Paragraph paragraph11 = new Paragraph() { RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "457FEDCC", TextId = "77777777" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize21 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties11.Append(runFonts21);
            paragraphMarkRunProperties11.Append(fontSize21);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript21);

            paragraphProperties11.Append(justification8);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run11 = new Run();

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts31 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize31 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript31 = new FontSizeComplexScript() { Val = "18" };

            runProperties11.Append(runFonts31);
            runProperties11.Append(fontSize31);
            runProperties11.Append(fontSizeComplexScript31);
            Text text11 = new Text();
            text11.Text = firstMember?.DutyStatus?.DutyStatusName ?? "N/A";

            run11.Append(runProperties11);
            run11.Append(text11);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run11);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            tableRow2.Append(tableCell6);
            tableRow2.Append(tableCell7);
            tableRow2.Append(tableCell8);
            tableRow2.Append(tableCell9);
            tableRow2.Append(tableCell10);
            tableRow2.Append(tableCell11);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            foreach (Member m in _members)
            {
                table1.Append(GenerateExceptionToDutyStatusTableRow(m));
            }
            return table1;
        }
        public TableRow GenerateExceptionToDutyStatusTableRow(Member _m)
        {
            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00FA49F6", RsidTableRowProperties = "00FA49F6", ParagraphId = "2BC69CB0", TextId = "1BE89A9B" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "985", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "1BD045BB", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = "Cpl.";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "2610", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "200FCDEF", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = "Lee, Chung";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "195C09BE", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(justification1);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = "2623";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders1);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "282BF12F", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(justification2);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = "A";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders2);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "38D057D7", TextId = "77777777" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(justification3);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = "M";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1980", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(leftBorder2);

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellBorders3);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00FA49F6", ParagraphId = "457FEDCC", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(justification4);
            paragraphProperties6.Append(paragraphMarkRunProperties6);


            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();
            text6.Text = _m?.DutyStatus?.DutyStatusName ?? "N/A";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            return tableRow1;
        }
        public TableRow GenerateFloatRightTableRow(Member _m)
        {
            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "007F3139", RsidTableRowProperties = "007F3139", ParagraphId = "5C8CFE19", TextId = "77777777" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "1165", Type = TableWidthUnitValues.Dxa };

            tableCellProperties1.Append(tableCellWidth1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "0008380A", RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "401AB99D", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0" };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "0008380A" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = _m?.Rank.RankShort ?? "N/A";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "2520", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "0008380A", RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "5C4F9619", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "0" };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(spacingBetweenLines2);
            paragraphProperties2.Append(paragraphMarkRunProperties2);
            ProofError proofError1 = new ProofError() { Type = ProofingErrorValues.SpellStart };

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = _m?.LastName ?? "N/A";

            run2.Append(runProperties2);
            run2.Append(text2);
            ProofError proofError2 = new ProofError() { Type = ProofingErrorValues.SpellEnd };

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts5);
            runProperties3.Append(fontSize5);
            runProperties3.Append(fontSizeComplexScript5);
            Text text3 = new Text();
            text3.Text = _m?.FirstName == null ? "" : $", {_m.FirstName}";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(proofError1);
            paragraph2.Append(run2);
            paragraph2.Append(proofError2);
            paragraph2.Append(run3);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "900", Type = TableWidthUnitValues.Dxa };

            tableCellProperties3.Append(tableCellWidth3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "743E23A8", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0" };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts6);
            paragraphMarkRunProperties3.Append(fontSize6);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript6);

            paragraphProperties3.Append(spacingBetweenLines3);
            paragraphProperties3.Append(justification1);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts7);
            runProperties4.Append(fontSize7);
            runProperties4.Append(fontSizeComplexScript7);
            Text text4 = new Text();
            text4.Text = _m?.IdNumber ?? "N/A";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run4);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "1170", Type = TableWidthUnitValues.Dxa };

            tableCellProperties4.Append(tableCellWidth4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "0EEAD76D", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0" };
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts8);
            paragraphMarkRunProperties4.Append(fontSize8);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript8);

            paragraphProperties4.Append(spacingBetweenLines4);
            paragraphProperties4.Append(justification2);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts9);
            runProperties5.Append(fontSize9);
            runProperties5.Append(fontSizeComplexScript9);
            Text text5 = new Text();
            text5.Text = _m?.Position?.Callsign ?? "NONE";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run5);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Bottom };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellVerticalAlignment1);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "62571995", TextId = "77777777" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0" };
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts10);
            paragraphMarkRunProperties5.Append(fontSize10);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript10);

            paragraphProperties5.Append(spacingBetweenLines5);
            paragraphProperties5.Append(justification3);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts11);
            runProperties6.Append(fontSize11);
            runProperties6.Append(fontSizeComplexScript11);
            Text text6 = new Text();
            text6.Text = _m?.Race.Abbreviation.ToString() ?? "-";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run6);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            tableCellProperties6.Append(tableCellWidth6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphAddition = "007F3139", RsidParagraphProperties = "007F3139", RsidRunAdditionDefault = "007F3139", ParagraphId = "427D50AE", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines() { After = "0" };
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts12);
            paragraphMarkRunProperties6.Append(fontSize12);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript12);

            paragraphProperties6.Append(spacingBetweenLines6);
            paragraphProperties6.Append(justification4);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts13);
            runProperties7.Append(fontSize13);
            runProperties7.Append(fontSizeComplexScript13);
            Text text7 = new Text();
            text7.Text = _m?.Gender.Abbreviation.ToString() ?? "-";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run7);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            return tableRow1;

        }
        public TableRow GenerateDemographicTableRow(string _rowName, List<Member> _members, bool shaded = false)
        {
            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "00417BB5", RsidTableRowProperties = "00417BB5" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "805", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);
            if (shaded == true)
            {
                Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties1.Append(shading1);
            }

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = _rowName;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellBorders1);
            tableCellProperties2.Append(tableCellVerticalAlignment2);
            if (shaded == true)
            {
                Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties2.Append(shading2);
            }

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(spacingBetweenLines2);
            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();

            // B/M Count
            text2.Text = _members.Where(x => x.GenderId == 2 && x.RaceId == 1).Count().ToString();

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);
            if (shaded == true)
            {
                Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties3.Append(shading3);
            }

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(spacingBetweenLines3);
            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();

            // H/M Count
            text3.Text = _members.Where(x => x.GenderId == 2 && (x.RaceId == 7 || x.RaceId == 8)).Count().ToString(); ;

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellVerticalAlignment4);
            if (shaded == true)
            {
                Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties4.Append(shading4);
            }

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(spacingBetweenLines4);
            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();

            // A/M Count
            text4.Text = _members.Where(x => x.GenderId == 2 && x.RaceId == 4).Count().ToString();

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "208", Type = TableWidthUnitValues.Pct };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders2);
            tableCellProperties5.Append(tableCellVerticalAlignment5);
            if (shaded == true)
            {
                Shading shading5 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties5.Append(shading5);
            }

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(spacingBetweenLines5);
            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();

            // W/M Count
            text5.Text = _members.Where(x => x.GenderId == 2 && x.RaceId == 3).Count().ToString();

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment6);
            if (shaded == true)
            {
                Shading shading6 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties6.Append(shading6);
            }
            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(spacingBetweenLines6);
            paragraphProperties6.Append(justification5);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();

            // B/F Count
            text6.Text = _members.Where(x => x.GenderId == 3 && x.RaceId == 1).Count().ToString(); ;

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(tableCellVerticalAlignment7);
            if (shaded == true)
            {
                Shading shading7 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties7.Append(shading7);
            }
            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines7 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties7.Append(runFonts13);
            paragraphMarkRunProperties7.Append(fontSize13);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript13);

            paragraphProperties7.Append(spacingBetweenLines7);
            paragraphProperties7.Append(justification6);
            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run7 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize14 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts14);
            runProperties7.Append(fontSize14);
            runProperties7.Append(fontSizeComplexScript14);
            Text text7 = new Text();

            // H/F Count
            text7.Text = _members.Where(x => x.GenderId == 3 && (x.RaceId == 7 || x.RaceId == 8)).Count().ToString();

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run7);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(tableCellVerticalAlignment8);
            if (shaded == true)
            {
                Shading shading8 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties8.Append(shading8);
            }
            Paragraph paragraph8 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines8 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize15 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties8.Append(runFonts15);
            paragraphMarkRunProperties8.Append(fontSize15);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript15);

            paragraphProperties8.Append(spacingBetweenLines8);
            paragraphProperties8.Append(justification7);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run8 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize16 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "18" };

            runProperties8.Append(runFonts16);
            runProperties8.Append(fontSize16);
            runProperties8.Append(fontSizeComplexScript16);
            Text text8 = new Text();

            // A/F Count
            text8.Text = _members.Where(x => x.GenderId == 3 && x.RaceId == 4).Count().ToString();

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run8);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "207", Type = TableWidthUnitValues.Pct };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(rightBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellBorders3);
            tableCellProperties9.Append(tableCellVerticalAlignment9);
            if (shaded == true)
            {
                Shading shading9 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties9.Append(shading9);
            }
            Paragraph paragraph9 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines9 = new SpacingBetweenLines() { After = "0", Line = "280", LineRule = LineSpacingRuleValues.Auto };
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize17 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties9.Append(runFonts17);
            paragraphMarkRunProperties9.Append(fontSize17);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript17);

            paragraphProperties9.Append(spacingBetweenLines9);
            paragraphProperties9.Append(justification8);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run9 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize18 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "18" };

            runProperties9.Append(runFonts18);
            runProperties9.Append(fontSize18);
            runProperties9.Append(fontSizeComplexScript18);
            Text text9 = new Text();

            // W/F Count
            text9.Text = _members.Where(x => x.GenderId == 3 && x.RaceId == 3).Count().ToString();

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment10 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellVerticalAlignment10);
            if (shaded == true)
            {
                Shading shading10 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties10.Append(shading10);
            }
            Paragraph paragraph10 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification9 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize19 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties10.Append(runFonts19);
            paragraphMarkRunProperties10.Append(fontSize19);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript19);

            paragraphProperties10.Append(spacingBetweenLines10);
            paragraphProperties10.Append(justification9);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run10 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize20 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "18" };

            runProperties10.Append(runFonts20);
            runProperties10.Append(fontSize20);
            runProperties10.Append(fontSizeComplexScript20);
            Text text10 = new Text();

            // B/All Count
            text10.Text = _members.Where(x => x.RaceId == 1).Count().ToString();

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run10);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment11 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties11.Append(tableCellWidth11);
            tableCellProperties11.Append(tableCellVerticalAlignment11);
            if (shaded == true)
            {
                Shading shading11 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties11.Append(shading11);
            }
            Paragraph paragraph11 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines11 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification10 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize21 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties11.Append(runFonts21);
            paragraphMarkRunProperties11.Append(fontSize21);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript21);

            paragraphProperties11.Append(spacingBetweenLines11);
            paragraphProperties11.Append(justification10);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run11 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts22 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize22 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript() { Val = "18" };

            runProperties11.Append(runFonts22);
            runProperties11.Append(fontSize22);
            runProperties11.Append(fontSizeComplexScript22);
            Text text11 = new Text();

            // H/All Count
            text11.Text = _members.Where(x => x.RaceId == 7 || x.RaceId == 8).Count().ToString();

            run11.Append(runProperties11);
            run11.Append(text11);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run11);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            TableCell tableCell12 = new TableCell();

            TableCellProperties tableCellProperties12 = new TableCellProperties();
            TableCellWidth tableCellWidth12 = new TableCellWidth() { Width = "206", Type = TableWidthUnitValues.Pct };
            TableCellVerticalAlignment tableCellVerticalAlignment12 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties12.Append(tableCellWidth12);
            tableCellProperties12.Append(tableCellVerticalAlignment12);
            if (shaded == true)
            {
                Shading shading12 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties12.Append(shading12);
            }
            Paragraph paragraph12 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties12 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines12 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification11 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties12 = new ParagraphMarkRunProperties();
            RunFonts runFonts23 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize23 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript23 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties12.Append(runFonts23);
            paragraphMarkRunProperties12.Append(fontSize23);
            paragraphMarkRunProperties12.Append(fontSizeComplexScript23);

            paragraphProperties12.Append(spacingBetweenLines12);
            paragraphProperties12.Append(justification11);
            paragraphProperties12.Append(paragraphMarkRunProperties12);

            Run run12 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties12 = new RunProperties();
            RunFonts runFonts24 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize24 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript24 = new FontSizeComplexScript() { Val = "18" };

            runProperties12.Append(runFonts24);
            runProperties12.Append(fontSize24);
            runProperties12.Append(fontSizeComplexScript24);
            Text text12 = new Text();

            // A/All count
            text12.Text = _members.Where(x => x.RaceId == 4).Count().ToString();

            run12.Append(runProperties12);
            run12.Append(text12);

            paragraph12.Append(paragraphProperties12);
            paragraph12.Append(run12);

            tableCell12.Append(tableCellProperties12);
            tableCell12.Append(paragraph12);

            TableCell tableCell13 = new TableCell();

            TableCellProperties tableCellProperties13 = new TableCellProperties();
            TableCellWidth tableCellWidth13 = new TableCellWidth() { Width = "207", Type = TableWidthUnitValues.Pct };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            RightBorder rightBorder3 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(rightBorder3);
            TableCellVerticalAlignment tableCellVerticalAlignment13 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties13.Append(tableCellWidth13);
            tableCellProperties13.Append(tableCellBorders4);
            tableCellProperties13.Append(tableCellVerticalAlignment13);
            if (shaded == true)
            {
                Shading shading13 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties13.Append(shading13);
            }
            Paragraph paragraph13 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties13 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines13 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification12 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties13 = new ParagraphMarkRunProperties();
            RunFonts runFonts25 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize25 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript25 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties13.Append(runFonts25);
            paragraphMarkRunProperties13.Append(fontSize25);
            paragraphMarkRunProperties13.Append(fontSizeComplexScript25);

            paragraphProperties13.Append(spacingBetweenLines13);
            paragraphProperties13.Append(justification12);
            paragraphProperties13.Append(paragraphMarkRunProperties13);

            Run run13 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties13 = new RunProperties();
            RunFonts runFonts26 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize26 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript26 = new FontSizeComplexScript() { Val = "18" };

            runProperties13.Append(runFonts26);
            runProperties13.Append(fontSize26);
            runProperties13.Append(fontSizeComplexScript26);
            Text text13 = new Text();

            // W/All Count
            text13.Text = _members.Where(x => x.RaceId == 3).Count().ToString();

            run13.Append(runProperties13);
            run13.Append(text13);

            paragraph13.Append(paragraphProperties13);
            paragraph13.Append(run13);

            tableCell13.Append(tableCellProperties13);
            tableCell13.Append(paragraph13);

            TableCell tableCell14 = new TableCell();

            TableCellProperties tableCellProperties14 = new TableCellProperties();
            TableCellWidth tableCellWidth14 = new TableCellWidth() { Width = "313", Type = TableWidthUnitValues.Pct };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            RightBorder rightBorder4 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders5.Append(rightBorder4);
            TableCellVerticalAlignment tableCellVerticalAlignment14 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties14.Append(tableCellWidth14);
            tableCellProperties14.Append(tableCellBorders5);
            tableCellProperties14.Append(tableCellVerticalAlignment14);
            if (shaded == true)
            {
                Shading shading14 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties14.Append(shading14);
            }
            Paragraph paragraph14 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties14 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines14 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification13 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties14 = new ParagraphMarkRunProperties();
            RunFonts runFonts27 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize27 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript27 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties14.Append(runFonts27);
            paragraphMarkRunProperties14.Append(fontSize27);
            paragraphMarkRunProperties14.Append(fontSizeComplexScript27);

            paragraphProperties14.Append(spacingBetweenLines14);
            paragraphProperties14.Append(justification13);
            paragraphProperties14.Append(paragraphMarkRunProperties14);

            Run run14 = new Run() { RsidRunProperties = "00417BB5" };

            RunProperties runProperties14 = new RunProperties();
            RunFonts runFonts28 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize28 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript28 = new FontSizeComplexScript() { Val = "18" };

            runProperties14.Append(runFonts28);
            runProperties14.Append(fontSize28);
            runProperties14.Append(fontSizeComplexScript28);
            Text text14 = new Text();

            // Total for Row
            text14.Text = _members.Count.ToString();

            run14.Append(runProperties14);
            run14.Append(text14);

            paragraph14.Append(paragraphProperties14);
            paragraph14.Append(run14);

            tableCell14.Append(tableCellProperties14);
            tableCell14.Append(paragraph14);

            TableCell tableCell15 = new TableCell();

            TableCellProperties tableCellProperties15 = new TableCellProperties();
            TableCellWidth tableCellWidth15 = new TableCellWidth() { Width = "1406", Type = TableWidthUnitValues.Pct };
            VerticalMerge verticalMerge1 = new VerticalMerge();

            TableCellBorders tableCellBorders6 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder5 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders6.Append(leftBorder2);
            tableCellBorders6.Append(rightBorder5);

            tableCellProperties15.Append(tableCellWidth15);
            tableCellProperties15.Append(verticalMerge1);
            tableCellProperties15.Append(tableCellBorders6);
            if (shaded == true)
            {
                Shading shading15 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tableCellProperties15.Append(shading15);
            }
            Paragraph paragraph15 = new Paragraph() { RsidParagraphMarkRevision = "00417BB5", RsidParagraphAddition = "00417BB5", RsidParagraphProperties = "00417BB5", RsidRunAdditionDefault = "00417BB5" };

            ParagraphProperties paragraphProperties15 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines15 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification14 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties15 = new ParagraphMarkRunProperties();
            RunFonts runFonts29 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize29 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript29 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties15.Append(runFonts29);
            paragraphMarkRunProperties15.Append(fontSize29);
            paragraphMarkRunProperties15.Append(fontSizeComplexScript29);

            paragraphProperties15.Append(spacingBetweenLines15);
            paragraphProperties15.Append(justification14);
            paragraphProperties15.Append(paragraphMarkRunProperties15);

            paragraph15.Append(paragraphProperties15);

            tableCell15.Append(tableCellProperties15);
            tableCell15.Append(paragraph15);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            tableRow1.Append(tableCell7);
            tableRow1.Append(tableCell8);
            tableRow1.Append(tableCell9);
            tableRow1.Append(tableCell10);
            tableRow1.Append(tableCell11);
            tableRow1.Append(tableCell12);
            tableRow1.Append(tableCell13);
            tableRow1.Append(tableCell14);
            tableRow1.Append(tableCell15);
            return tableRow1;
        }
        public Paragraph GenerateColumnBreakParagraph()
        {
            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "004B7D8C", RsidRunAdditionDefault = "0091509C", ParagraphId = "026EF1AF", TextId = "442207E0" };

            Run run1 = new Run();
            Break break1 = new Break() { Type = BreakValues.Column };

            run1.Append(break1);

            paragraph1.Append(run1);
            return paragraph1;
        }
        public Paragraph GenerateSectionBreakParagraph()
        {
            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "0091509C", RsidRunAdditionDefault = "0091509C", ParagraphId = "05968BDE", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            SectionProperties sectionProperties1 = new SectionProperties() { RsidR = "0091509C", RsidSect = "00755D44" };
            SectionType sectionType1 = new SectionType() { Val = SectionMarkValues.Continuous };
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)15840U, Height = (UInt32Value)12240U, Orient = PageOrientationValues.Landscape };
            PageMargin pageMargin1 = new PageMargin() { Top = 720, Right = (UInt32Value)720U, Bottom = 720, Left = (UInt32Value)720U, Header = (UInt32Value)288U, Footer = (UInt32Value)0U, Gutter = (UInt32Value)0U };
            Columns columns1 = new Columns() { Space = "720", ColumnCount = 2 };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(sectionType1);
            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);

            paragraphProperties1.Append(sectionProperties1);

            paragraph1.Append(paragraphProperties1);
            return paragraph1;
        }
        public SectionProperties GeneratePageBreakSectionProperties()
        {
            SectionProperties sectionProperties1 = new SectionProperties() { RsidR = "0091509C", RsidSect = "00DB001A" };
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)15840U, Height = (UInt32Value)12240U, Orient = PageOrientationValues.Landscape };
            PageMargin pageMargin1 = new PageMargin() { Top = 720, Right = (UInt32Value)720U, Bottom = 720, Left = (UInt32Value)720U, Header = (UInt32Value)288U, Footer = (UInt32Value)0U, Gutter = (UInt32Value)0U };
            Columns columns1 = new Columns() { Space = "720" };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);
            return sectionProperties1;
        }
        public Paragraph GenerateTwoColumnParagraph()
        {
            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            SectionProperties sectionProperties1 = new SectionProperties();
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)15840U, Height = (UInt32Value)12240U, Orient = PageOrientationValues.Landscape };
            PageMargin pageMargin1 = new PageMargin() { Top = 720, Right = (UInt32Value)720U, Bottom = 720, Left = (UInt32Value)720U };
            Columns columns1 = new Columns() { Space = "720" };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);

            paragraphProperties1.Append(sectionProperties1);

            paragraph1.Append(paragraphProperties1);
            return paragraph1;
        }
        public Table GenerateComponentTable(Component _c)
        {
            Member supervisor = _c?.Positions?.FirstOrDefault(x => x.IsManager).Members.FirstOrDefault();
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "7105", Type = TableWidthUnitValues.Dxa };
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "985" };
            GridColumn gridColumn2 = new GridColumn() { Width = "2610" };
            GridColumn gridColumn3 = new GridColumn() { Width = "720" };
            GridColumn gridColumn4 = new GridColumn() { Width = "450" };
            GridColumn gridColumn5 = new GridColumn() { Width = "360" };
            GridColumn gridColumn6 = new GridColumn() { Width = "1980" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00FA49F6", RsidTableRowProperties = "006C7CA8", ParagraphId = "2A53F4F2", TextId = "77777777" };

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)503U };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "3595", Type = TableWidthUnitValues.Dxa };
            GridSpan gridSpan1 = new GridSpan() { Val = 2 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(gridSpan1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00E26227", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D96530", ParagraphId = "10CCF41A", TextId = "14055416" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold1 = new Bold();
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold2 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();

            text1.Text = $"{_c.Name.ToUpper()} ({_c.GetManagerCount()} and {_c.GetWorkerCount()})";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "21BD502E", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = "ID";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders1);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "76B9BA2B", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = "R";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders2);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "4E27EC12", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run() { RsidRunProperties = "00443C9F" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = "S";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "1980", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(leftBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders3);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D96530", ParagraphId = "6C985078", TextId = "171412A8" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = "NOTE";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);

            TableRow tableRow2 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00FA49F6", RsidTableRowProperties = "00FA49F6", ParagraphId = "6A3742A3", TextId = "77777777" };

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "985", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "71E8AA5F", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();
            text6.Text = supervisor?.Rank?.RankShort ?? "N/A";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "2610", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(tableCellVerticalAlignment7);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "05DD0324", TextId = "77777777" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts13 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize13 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties7.Append(runFonts13);
            paragraphMarkRunProperties7.Append(fontSize13);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript13);

            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts14 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize14 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript() { Val = "18" };

            runProperties7.Append(runFonts14);
            runProperties7.Append(fontSize14);
            runProperties7.Append(fontSizeComplexScript14);
            Text text7 = new Text();
            text7.Text = supervisor?.GetLastNameFirstName() ?? "N/A";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run7);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "720", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(tableCellVerticalAlignment8);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "0ACE1CAD", TextId = "77777777" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            Justification justification5 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts15 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize15 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties8.Append(runFonts15);
            paragraphMarkRunProperties8.Append(fontSize15);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript15);

            paragraphProperties8.Append(justification5);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts16 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize16 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript() { Val = "18" };

            runProperties8.Append(runFonts16);
            runProperties8.Append(fontSize16);
            runProperties8.Append(fontSizeComplexScript16);
            Text text8 = new Text();
            text8.Text = supervisor?.IdNumber ?? "N/A";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run8);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "450", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(rightBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellBorders4);
            tableCellProperties9.Append(tableCellVerticalAlignment9);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "202ECCDA", TextId = "77777777" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            Justification justification6 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts17 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize17 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties9.Append(runFonts17);
            paragraphMarkRunProperties9.Append(fontSize17);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript17);

            paragraphProperties9.Append(justification6);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run9 = new Run();

            RunProperties runProperties9 = new RunProperties();
            RunFonts runFonts18 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize18 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript() { Val = "18" };

            runProperties9.Append(runFonts18);
            runProperties9.Append(fontSize18);
            runProperties9.Append(fontSizeComplexScript18);
            Text text9 = new Text();
            text9.Text = supervisor?.Race?.Abbreviation.ToString() ?? "-";

            run9.Append(runProperties9);
            run9.Append(text9);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "360", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders5.Append(leftBorder3);
            TableCellVerticalAlignment tableCellVerticalAlignment10 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(tableCellBorders5);
            tableCellProperties10.Append(tableCellVerticalAlignment10);

            Paragraph paragraph10 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "3E757CEB", TextId = "77777777" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            Justification justification7 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts19 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize19 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties10.Append(runFonts19);
            paragraphMarkRunProperties10.Append(fontSize19);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript19);

            paragraphProperties10.Append(justification7);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run10 = new Run();

            RunProperties runProperties10 = new RunProperties();
            RunFonts runFonts20 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize20 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript() { Val = "18" };

            runProperties10.Append(runFonts20);
            runProperties10.Append(fontSize20);
            runProperties10.Append(fontSizeComplexScript20);
            Text text10 = new Text();
            text10.Text = supervisor?.Gender?.Abbreviation.ToString() ?? "-";

            run10.Append(runProperties10);
            run10.Append(text10);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run10);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "1980", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders6 = new TableCellBorders();
            LeftBorder leftBorder4 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders6.Append(leftBorder4);

            tableCellProperties11.Append(tableCellWidth11);
            tableCellProperties11.Append(tableCellBorders6);

            Paragraph paragraph11 = new Paragraph() { RsidParagraphAddition = "00FA49F6", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "000A35FD", ParagraphId = "3C462647", TextId = "77777777" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            Justification justification8 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts21 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize21 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties11.Append(runFonts21);
            paragraphMarkRunProperties11.Append(fontSize21);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript21);

            paragraphProperties11.Append(justification8);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run11 = new Run();

            RunProperties runProperties11 = new RunProperties();
            RunFonts runFonts22 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize22 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript() { Val = "18" };

            runProperties11.Append(runFonts22);
            runProperties11.Append(fontSize22);
            runProperties11.Append(fontSizeComplexScript22);
            Text text11 = new Text();
            text11.Text = "Supervisor";

            run11.Append(runProperties11);
            run11.Append(text11);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run11);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            tableRow2.Append(tableCell6);
            tableRow2.Append(tableCell7);
            tableRow2.Append(tableCell8);
            tableRow2.Append(tableCell9);
            tableRow2.Append(tableCell10);
            tableRow2.Append(tableCell11);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            if(_c.Positions != null && _c.Positions.Count > 0)
            {
                foreach (Position p in _c.Positions)
                {
                    if (!p.IsManager)
                    {
                        foreach (Member m in p.Members)
                        {
                            table1.Append(GenerateComponentTableRow(m));
                        }
                    }                    
                }
            }            
            return table1;

        }
        public Table GenerateFullWidthTable(Component _c)
        {
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid1" };
            TableWidth tableWidth1 = new TableWidth() { Width = "14665", Type = TableWidthUnitValues.Dxa };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "000000", ThemeColor = ThemeColorValues.Text1, Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "000000", ThemeColor = ThemeColorValues.Text1, Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "000000", ThemeColor = ThemeColorValues.Text1, Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "000000", ThemeColor = ThemeColorValues.Text1, Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "000000", ThemeColor = ThemeColorValues.Text1, Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "000000", ThemeColor = ThemeColorValues.Text1, Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLayout tableLayout1 = new TableLayout() { Type = TableLayoutValues.Fixed };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLayout1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "14238" };

            tableGrid1.Append(gridColumn1);

            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00D76842", RsidTableRowAddition = "00D76842", RsidTableRowProperties = "00997057" };

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)530U };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "14238", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00D76842", RsidParagraphAddition = "00D76842", RsidParagraphProperties = "00997057", RsidRunAdditionDefault = "00D76842" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacing1 = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(spacing1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "00D76842" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(bold1);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            string displaytext = _c.Name.ToUpper();
            Position manager = _c?.Positions?.Where(x => x.IsManager == true).FirstOrDefault();
            if (manager != null)
            {
                Member super = manager?.Members.FirstOrDefault();
                if (super != null)
                {
                    displaytext += " " + manager.Name + ": " + super.GetTitleName();
                }
            }
            text1.Text = displaytext;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            return table1;



        }
        public TableRow GenerateComponentTableRow(Member _m)
        {
            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00443C9F", RsidTableRowAddition = "00D317D3", RsidTableRowProperties = "00E369F0" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "1183", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize1 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize2 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "18" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = _m?.Rank.RankShort ?? "N/A";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "2249", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize3 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize4 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "18" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = _m.GetLastNameFirstName();

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "612", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize5 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(justification1);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize6 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "18" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = _m?.IdNumber ?? "N/A";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "832", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize7 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(justification2);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize8 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "18" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = _m?.Race.Abbreviation.ToString() ?? "-" ;

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "380", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders1);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize9 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(justification3);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize10 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "18" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = _m.Gender.Abbreviation.ToString() ?? "-";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "355", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder1);
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellBorders2);
            tableCellProperties6.Append(tableCellVerticalAlignment6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00443C9F", RsidParagraphAddition = "00D317D3", RsidParagraphProperties = "00D7521E", RsidRunAdditionDefault = "00D317D3" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts11 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize11 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript() { Val = "18" };

            paragraphMarkRunProperties6.Append(runFonts11);
            paragraphMarkRunProperties6.Append(fontSize11);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript11);

            paragraphProperties6.Append(justification4);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Trebuchet MS", HighAnsi = "Trebuchet MS" };
            FontSize fontSize12 = new FontSize() { Val = "18" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript() { Val = "18" };

            runProperties6.Append(runFonts12);
            runProperties6.Append(fontSize12);
            runProperties6.Append(fontSizeComplexScript12);
            Text text6 = new Text();
            text6.Text = _m.DutyStatus?.DutyStatusName ?? "-";

            run6.Append(runProperties6);
            run6.Append(text6);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);
            paragraph6.Append(bookmarkStart1);
            paragraph6.Append(bookmarkEnd1);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            return tableRow1;
        }

    }
}
 