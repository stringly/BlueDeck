using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BlueDeck.Models.Types;
using BlueDeck.Models.Enums;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.Models.DocGenerators
{
    /// <summary>
    /// 
    /// </summary>
    public class LineupGenerator
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public LineupGeneratorViewModel Model { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupGenerator"/> class.
        /// </summary>
        /// <param name="_model">The model.</param>
        public LineupGenerator(LineupGeneratorViewModel _model)
        {
            Model = _model;
        }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        public MemoryStream Generate()
        {
            var mem = new MemoryStream();
            byte[] byteArray = File.ReadAllBytes("Templates/Lineup_Template.docx");
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(mem, true))
            {
                int onDutyMembers = 0;
                foreach (LineupMember m in Model.Members)
                {
                    if (!String.IsNullOrEmpty(m.Callsign))
                    {
                        onDutyMembers++;
                    }
                }
                // retrieve MainPart
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;

                // retrieve header table
                Table headerTable = mainPart.HeaderParts.ElementAt(0).RootElement.Elements<Table>().ElementAt(0);

                // Set District Commander Name
                RunProperties runProperties1 = new RunProperties();
                FontSize fontSize1 = new FontSize() { Val = "28" };
                RunFonts runFonts1 = new RunFonts() { Ascii = "Times New Roman" };                
                runProperties1.Append(fontSize1);
                runProperties1.Append(runFonts1);
                Run run1 = new Run();
                run1.Append(runProperties1);
                Text text1 = new Text(Model.CommanderName);
                run1.Append(text1);
                headerTable.Elements<TableRow>().ElementAt(1)
                    .Elements<TableCell>().ElementAt(1)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run1);

                // Set District Commander Title
                RunProperties runProperties2 = new RunProperties();
                FontSize fontSize2 = new FontSize() { Val = "28" };
                RunFonts runFonts2 = new RunFonts() { Ascii = "Times New Roman" };                
                runProperties2.Append(fontSize2);
                runProperties2.Append(runFonts2);
                Run run2 = new Run();
                run2.Append(runProperties2);
                Text text2 = new Text(Model.CommanderTitle);
                run2.Append(text2);
                headerTable.Elements<TableRow>().ElementAt(2)
                    .Elements<TableCell>().ElementAt(1)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run2);
                // Retrieve "Assistant" table, which will contain the Assistant Commander's Details and 
                // the Component Name/Lineup Date
                Table assistantCommanderTable = mainPart.Document.Body.Elements<Table>().ElementAt(0);

                // Set Assistant Commander Name
                RunProperties runProperties3 = new RunProperties();
                FontSize fontSize3 = new FontSize() { Val = "24" };
                RunFonts runFonts3 = new RunFonts() { Ascii = "Time New Roman" };
                runProperties3.Append(fontSize3);
                runProperties3.Append(runFonts3);
                Run run3 = new Run();
                run3.Append(runProperties3);
                Text text3 = new Text(Model.AssistantCommanderName);
                run3.Append(text3);
                assistantCommanderTable.Elements<TableRow>().ElementAt(0)
                    .Elements<TableCell>().ElementAt(0)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run3);

                // Set Assistant Commander Title
                RunProperties runProperties4 = new RunProperties();
                FontSize fontSize4 = new FontSize() { Val = "24" };
                RunFonts runFonts4 = new RunFonts() { Ascii = "Times New Roman" };
                runProperties4.Append(fontSize4);
                runProperties4.Append(runFonts4);
                Run run4 = new Run();
                run4.Append(runProperties4);
                Text text4 = new Text(Model.AssistantCommanderTitle);
                run4.Append(text4);
                assistantCommanderTable.Elements<TableRow>().ElementAt(1)
                    .Elements<TableCell>().ElementAt(0)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run4);

                // Set Component Name
                RunProperties runProperties5 = new RunProperties();
                FontSize fontSize5 = new FontSize() { Val = "40" };
                RunFonts runFonts5 = new RunFonts() { Ascii = "Times New Roman" };
                Bold bold5 = new Bold();
                runProperties5.Append(fontSize5);
                runProperties5.Append(runFonts5);
                runProperties5.Append(bold5);
                Run run5 = new Run();
                run5.Append(runProperties5);
                Text text5 = new Text(Model.ComponentName);
                run5.Append(text5);
                assistantCommanderTable.Elements<TableRow>().ElementAt(2)
                    .Elements<TableCell>().ElementAt(0)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run5);
                
                // Set Lineup Date
                RunProperties runProperties6 = new RunProperties();
                FontSize fontSize6 = new FontSize() { Val = "40" };
                RunFonts runFonts6 = new RunFonts() { Ascii = "Times New Roman" };
                Bold bold6 = new Bold();
                runProperties6.Append(fontSize6);
                runProperties6.Append(runFonts6);
                runProperties6.Append(bold6);
                Run run6 = new Run();
                run6.Append(runProperties6);
                Text text6 = new Text(Model.LineupDate.ToLongDateString());
                run6.Append(text6);
                assistantCommanderTable.Elements<TableRow>().ElementAt(2)
                    .Elements<TableCell>().ElementAt(1)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run6);
                
                // Retrieve the Lineup Table
                Table lineupTable = mainPart.Document.Body.Elements<Table>().ElementAt(1);

                // Set Shift Commander fields
                TableRow row = lineupTable.Elements<TableRow>().ElementAt(2);
                SetExistingTableRow(row, Model.ShiftCommander);

                // Set OIC Fields
                row = lineupTable.Elements<TableRow>().ElementAt(5);
                SetExistingTableRow(row, Model.OIC);

                // Set First Member... this is used instead of adding a row for the first member because of Word Table Formatting issues
                
                if (Model.Members.Count > 0)
                {
                    row = lineupTable.Elements<TableRow>().ElementAt(8);
                    SetExistingTableRow(row, Model.Members.First());
                    Model.Members.Remove(Model.Members.First());
                    if (Model.Members.Count > 0 ) // if there are still members to render
                    {
                        foreach (LineupMember m in Model.Members)
                        {
                            lineupTable.Append(AddMemberTableRow(m));
                        }                        
                    }                         
                }
                // retrieve header table
                Table footerTable = mainPart.FooterParts.ElementAt(0).RootElement.Elements<Table>().ElementAt(0);

                // Set Strength Count
                RunProperties runProperties7 = new RunProperties();
                FontSize fontSize7 = new FontSize() { Val = "28" };
                RunFonts runFonts7 = new RunFonts() { Ascii = "Times New Roman" };
                Bold bold7 = new Bold();
                runProperties7.Append(fontSize7);
                runProperties7.Append(runFonts7);
                runProperties7.Append(bold7);
                Run run7 = new Run();
                run7.Append(runProperties7);
                string strength = !String.IsNullOrEmpty(Model.OIC.Callsign) ? $"1 and {onDutyMembers}" : $"1 and {(onDutyMembers -1)}";
                Text text7 = new Text(strength);
                run7.Append(text7);
                footerTable.Elements<TableRow>().ElementAt(0)
                    .Elements<TableCell>().ElementAt(0)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run7);

            }

            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }

        private void SetExistingTableRow(TableRow row, LineupMember member)
        {
            // Callsign
            RunProperties runProperties7 = new RunProperties();
            FontSize fontSize7 = new FontSize() { Val = "20" };
            RunFonts runFonts7 = new RunFonts() { Ascii = "Times New Roman" };
            runProperties7.Append(fontSize7);
            runProperties7.Append(runFonts7);
            Run run7 = new Run();
            run7.Append(runProperties7);
            Text text7 = new Text(member.Callsign);
            run7.Append(text7);
                
            row.Elements<TableCell>().ElementAt(0)
                .Elements<Paragraph>().ElementAt(0)
                    .Append(run7);

            // ID
            RunProperties runProperties8 = new RunProperties();
            FontSize fontSize8 = new FontSize() { Val = "20" };
            RunFonts runFonts8 = new RunFonts() { Ascii = "Times New Roman" };
            runProperties8.Append(fontSize8);
            runProperties8.Append(runFonts8);
            Run run8 = new Run();
            run8.Append(runProperties8);
            Text text8 = new Text(member.BadgeNumber);
            run8.Append(text8);
                
            row.Elements<TableCell>().ElementAt(1)
                .Elements<Paragraph>().ElementAt(0)
                    .Append(run8);
                
            // Rank
            RunProperties runProperties9 = new RunProperties();
            FontSize fontSize9 = new FontSize() { Val = "20" };
            RunFonts runFonts9 = new RunFonts() { Ascii = "Times New Roman" };
            runProperties9.Append(fontSize9);
            runProperties9.Append(runFonts9);
            Run run9 = new Run();
            run9.Append(runProperties9);
            Text text9 = new Text(member.Rank);
            run9.Append(text9);
            
            row.Elements<TableCell>().ElementAt(2)
                .Elements<Paragraph>().ElementAt(0)
                    .Append(run9);

            // Name
            RunProperties runProperties10 = new RunProperties();
            FontSize fontSize10 = new FontSize() { Val = "20" };
            RunFonts runFonts10 = new RunFonts() { Ascii = "Times New Roman" };
            runProperties10.Append(fontSize10);
            runProperties10.Append(runFonts10);
            Run run10 = new Run();
            run10.Append(runProperties10);
            Text text10 = new Text(member.MemberName);
            run10.Append(text10);
            row.Elements<TableCell>().ElementAt(3)
                .Elements<Paragraph>().ElementAt(0)
                    .Append(run10);

            // Cruiser Number
            RunProperties runProperties11 = new RunProperties();
            FontSize fontSize11 = new FontSize() { Val = "20" };
            RunFonts runFonts11 = new RunFonts() { Ascii = "Times New Roman" };
            runProperties11.Append(fontSize11);
            runProperties11.Append(runFonts11);
            Run run11 = new Run();
            run11.Append(runProperties11);
            Text text11 = new Text(member.CruiserNumber);
            run11.Append(text11);
            
            row.Elements<TableCell>().ElementAt(4)
                .Elements<Paragraph>().ElementAt(0)
                    .Append(run11);

            // IsOverlap
            if (member.IsOverlap == true)
            {
                TableCellProperties tableCellProperties1 = new TableCellProperties();
                Shading shading1 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "F29530", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "BF" };
                tableCellProperties1.Append(shading1);
                
                row.Elements<TableCell>().ElementAt(5)
                    .Append(tableCellProperties1);
            }

            // Shift Working
            TableCellProperties tableCellProperties2 = new TableCellProperties();
                Shading shading2 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "F29530", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "BF" };
                tableCellProperties2.Append(shading2);

            switch (member.ShiftWorking)
            {                    
                case 1: // mids
                    row.Elements<TableCell>().ElementAt(8)
                        .Append(tableCellProperties2);                       
                    break;
                case 2: // days
                    row.Elements<TableCell>().ElementAt(6)
                        .Append(tableCellProperties2);
                    break;
                case 3: // eves
                    row.Elements<TableCell>().ElementAt(7)
                        .Append(tableCellProperties2);
                    break;
                default: // rdo
                    row.Elements<TableCell>().ElementAt(9)
                        .Append(tableCellProperties2);  
                    break;
            }
                
            // MVS
            TableCellProperties tableCellProperties3 = new TableCellProperties();
                Shading shading3 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "F29530", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "BF" };
                tableCellProperties3.Append(shading3);
            switch (member.MVSStatus)
            {
                case 0: // no MVS
                    row.Elements<TableCell>().ElementAt(12)
                        .Append(tableCellProperties3);  
                    break;
                case 1: // installed, but non-functional
                     row.Elements<TableCell>().ElementAt(11)
                         .Append(tableCellProperties3);
                    break;
                case 2: // functional
                     row.Elements<TableCell>().ElementAt(10)
                         .Append(tableCellProperties3);
                    break;
            }

            // Status
            RunProperties runProperties12 = new RunProperties();
            FontSize fontSize12 = new FontSize() { Val = "20" };
            RunFonts runFonts12 = new RunFonts() { Ascii = "Times New Roman" };
            runProperties12.Append(fontSize12);
            runProperties12.Append(runFonts12);
            Run run12 = new Run();
            run12.Append(runProperties12);
            Text text12 = new Text(member.StatusNote);
            run12.Append(text12);
            
            row.Elements<TableCell>().ElementAt(13)
                .Elements<Paragraph>().ElementAt(0)
                    .Append(run12);
        }

        private TableRow AddMemberTableRow(LineupMember member)
        {
            TableRow row = GenerateLineupTableRow();
            SetExistingTableRow(row, member);            
            return row;
        }
        private TableRow GenerateLineupTableRow()
        {
            TableRow tableRow1 = new TableRow(){ RsidTableRowAddition = "006D7C52", RsidTableRowProperties = "004E447F", ParagraphId = "11192864", TextId = "77777777" };

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight(){ Val = (UInt32Value)278U };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth(){ Width = "900", Type = TableWidthUnitValues.Dxa };
            Shading shading1 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(shading1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "0C784482", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            Justification justification1 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize1 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            paragraph1.Append(paragraphProperties1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth(){ Width = "810", Type = TableWidthUnitValues.Dxa };
            Shading shading2 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(shading2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "7CE8EFA9", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification2 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts2 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize2 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties2.Append(runFonts2);
            paragraphMarkRunProperties2.Append(fontSize2);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript2);

            paragraphProperties2.Append(justification2);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            paragraph2.Append(paragraphProperties2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth(){ Width = "900", Type = TableWidthUnitValues.Dxa };
            Shading shading3 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(shading3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "0F3F2EB1", TextId = "77777777" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification3 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize3 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties3.Append(runFonts3);
            paragraphMarkRunProperties3.Append(fontSize3);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript3);

            paragraphProperties3.Append(justification3);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            paragraph3.Append(paragraphProperties3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth(){ Width = "3150", Type = TableWidthUnitValues.Dxa };
            Shading shading4 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(shading4);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "05EEFF5E", TextId = "77777777" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification4 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts4 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize4 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties4.Append(runFonts4);
            paragraphMarkRunProperties4.Append(fontSize4);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript4);

            paragraphProperties4.Append(justification4);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            paragraph4.Append(paragraphProperties4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth(){ Width = "900", Type = TableWidthUnitValues.Dxa };
            Shading shading5 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(shading5);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "3CC75C11", TextId = "77777777" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification5 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize5 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties5.Append(runFonts5);
            paragraphMarkRunProperties5.Append(fontSize5);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript5);

            paragraphProperties5.Append(justification5);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            paragraph5.Append(paragraphProperties5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth(){ Width = "630", Type = TableWidthUnitValues.Dxa };
            Shading shading6 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(shading6);
            tableCellProperties6.Append(tableCellVerticalAlignment6);

            Paragraph paragraph6 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "4723C461", TextId = "77777777" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            Justification justification6 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            RunFonts runFonts6 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize6 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties6.Append(runFonts6);
            paragraphMarkRunProperties6.Append(fontSize6);
            paragraphMarkRunProperties6.Append(fontSizeComplexScript6);

            paragraphProperties6.Append(justification6);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts7 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize7 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript(){ Val = "20" };

            runProperties1.Append(runFonts7);
            runProperties1.Append(fontSize7);
            runProperties1.Append(fontSizeComplexScript7);
            Text text1 = new Text();
            text1.Text = "O/L";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run1);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth(){ Width = "517", Type = TableWidthUnitValues.Dxa };
            Shading shading7 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment7 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(shading7);
            tableCellProperties7.Append(tableCellVerticalAlignment7);

            Paragraph paragraph7 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "42318CAA", TextId = "77777777" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();
            Justification justification7 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties7 = new ParagraphMarkRunProperties();
            RunFonts runFonts8 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize8 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties7.Append(runFonts8);
            paragraphMarkRunProperties7.Append(fontSize8);
            paragraphMarkRunProperties7.Append(fontSizeComplexScript8);

            paragraphProperties7.Append(justification7);
            paragraphProperties7.Append(paragraphMarkRunProperties7);

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts9 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize9 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript(){ Val = "20" };

            runProperties2.Append(runFonts9);
            runProperties2.Append(fontSize9);
            runProperties2.Append(fontSizeComplexScript9);
            Text text2 = new Text();
            text2.Text = "D";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph7.Append(paragraphProperties7);
            paragraph7.Append(run2);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth(){ Width = "518", Type = TableWidthUnitValues.Dxa };
            Shading shading8 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment8 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(shading8);
            tableCellProperties8.Append(tableCellVerticalAlignment8);

            Paragraph paragraph8 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "01EE1DEB", TextId = "77777777" };

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            Justification justification8 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            RunFonts runFonts10 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize10 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties8.Append(runFonts10);
            paragraphMarkRunProperties8.Append(fontSize10);
            paragraphMarkRunProperties8.Append(fontSizeComplexScript10);

            paragraphProperties8.Append(justification8);
            paragraphProperties8.Append(paragraphMarkRunProperties8);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts11 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize11 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript11 = new FontSizeComplexScript(){ Val = "20" };

            runProperties3.Append(runFonts11);
            runProperties3.Append(fontSize11);
            runProperties3.Append(fontSizeComplexScript11);
            Text text3 = new Text();
            text3.Text = "E";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph8.Append(paragraphProperties8);
            paragraph8.Append(run3);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth(){ Width = "517", Type = TableWidthUnitValues.Dxa };
            Shading shading9 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment9 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(shading9);
            tableCellProperties9.Append(tableCellVerticalAlignment9);

            Paragraph paragraph9 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "26FE70C7", TextId = "77777777" };

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            Justification justification9 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties9 = new ParagraphMarkRunProperties();
            RunFonts runFonts12 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize12 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript12 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties9.Append(runFonts12);
            paragraphMarkRunProperties9.Append(fontSize12);
            paragraphMarkRunProperties9.Append(fontSizeComplexScript12);

            paragraphProperties9.Append(justification9);
            paragraphProperties9.Append(paragraphMarkRunProperties9);

            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts13 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize13 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript13 = new FontSizeComplexScript(){ Val = "20" };

            runProperties4.Append(runFonts13);
            runProperties4.Append(fontSize13);
            runProperties4.Append(fontSizeComplexScript13);
            Text text4 = new Text();
            text4.Text = "M";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph9.Append(paragraphProperties9);
            paragraph9.Append(run4);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth(){ Width = "518", Type = TableWidthUnitValues.Dxa };
            Shading shading10 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment10 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties10.Append(tableCellWidth10);
            tableCellProperties10.Append(shading10);
            tableCellProperties10.Append(tableCellVerticalAlignment10);

            Paragraph paragraph10 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "0A3942D0", TextId = "77777777" };

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            Justification justification10 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties10 = new ParagraphMarkRunProperties();
            RunFonts runFonts14 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize14 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript14 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties10.Append(runFonts14);
            paragraphMarkRunProperties10.Append(fontSize14);
            paragraphMarkRunProperties10.Append(fontSizeComplexScript14);

            paragraphProperties10.Append(justification10);
            paragraphProperties10.Append(paragraphMarkRunProperties10);

            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts15 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize15 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript15 = new FontSizeComplexScript(){ Val = "20" };

            runProperties5.Append(runFonts15);
            runProperties5.Append(fontSize15);
            runProperties5.Append(fontSizeComplexScript15);
            Text text5 = new Text();
            text5.Text = "O";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph10.Append(paragraphProperties10);
            paragraph10.Append(run5);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth(){ Width = "690", Type = TableWidthUnitValues.Dxa };
            Shading shading11 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment11 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties11.Append(tableCellWidth11);
            tableCellProperties11.Append(shading11);
            tableCellProperties11.Append(tableCellVerticalAlignment11);

            Paragraph paragraph11 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "51C4D8BD", TextId = "77777777" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            Justification justification11 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties11 = new ParagraphMarkRunProperties();
            RunFonts runFonts16 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize16 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript16 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties11.Append(runFonts16);
            paragraphMarkRunProperties11.Append(fontSize16);
            paragraphMarkRunProperties11.Append(fontSizeComplexScript16);

            paragraphProperties11.Append(justification11);
            paragraphProperties11.Append(paragraphMarkRunProperties11);

            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();
            RunFonts runFonts17 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize17 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript17 = new FontSizeComplexScript(){ Val = "20" };

            runProperties6.Append(runFonts17);
            runProperties6.Append(fontSize17);
            runProperties6.Append(fontSizeComplexScript17);
            Text text6 = new Text();
            text6.Text = "F";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph11.Append(paragraphProperties11);
            paragraph11.Append(run6);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph11);

            TableCell tableCell12 = new TableCell();

            TableCellProperties tableCellProperties12 = new TableCellProperties();
            TableCellWidth tableCellWidth12 = new TableCellWidth(){ Width = "690", Type = TableWidthUnitValues.Dxa };
            Shading shading12 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment12 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties12.Append(tableCellWidth12);
            tableCellProperties12.Append(shading12);
            tableCellProperties12.Append(tableCellVerticalAlignment12);

            Paragraph paragraph12 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "07BBA6D3", TextId = "77777777" };

            ParagraphProperties paragraphProperties12 = new ParagraphProperties();
            Justification justification12 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties12 = new ParagraphMarkRunProperties();
            RunFonts runFonts18 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize18 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript18 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties12.Append(runFonts18);
            paragraphMarkRunProperties12.Append(fontSize18);
            paragraphMarkRunProperties12.Append(fontSizeComplexScript18);

            paragraphProperties12.Append(justification12);
            paragraphProperties12.Append(paragraphMarkRunProperties12);

            Run run7 = new Run();

            RunProperties runProperties7 = new RunProperties();
            RunFonts runFonts19 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize19 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript19 = new FontSizeComplexScript(){ Val = "20" };

            runProperties7.Append(runFonts19);
            runProperties7.Append(fontSize19);
            runProperties7.Append(fontSizeComplexScript19);
            Text text7 = new Text();
            text7.Text = "N/F";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph12.Append(paragraphProperties12);
            paragraph12.Append(run7);

            tableCell12.Append(tableCellProperties12);
            tableCell12.Append(paragraph12);

            TableCell tableCell13 = new TableCell();

            TableCellProperties tableCellProperties13 = new TableCellProperties();
            TableCellWidth tableCellWidth13 = new TableCellWidth(){ Width = "690", Type = TableWidthUnitValues.Dxa };
            Shading shading13 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment13 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties13.Append(tableCellWidth13);
            tableCellProperties13.Append(shading13);
            tableCellProperties13.Append(tableCellVerticalAlignment13);

            Paragraph paragraph13 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "44FBB02A", TextId = "77777777" };

            ParagraphProperties paragraphProperties13 = new ParagraphProperties();
            Justification justification13 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties13 = new ParagraphMarkRunProperties();
            RunFonts runFonts20 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize20 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript20 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties13.Append(runFonts20);
            paragraphMarkRunProperties13.Append(fontSize20);
            paragraphMarkRunProperties13.Append(fontSizeComplexScript20);

            paragraphProperties13.Append(justification13);
            paragraphProperties13.Append(paragraphMarkRunProperties13);

            Run run8 = new Run();

            RunProperties runProperties8 = new RunProperties();
            RunFonts runFonts21 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize21 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript21 = new FontSizeComplexScript(){ Val = "20" };

            runProperties8.Append(runFonts21);
            runProperties8.Append(fontSize21);
            runProperties8.Append(fontSizeComplexScript21);
            Text text8 = new Text();
            text8.Text = "N/S";

            run8.Append(runProperties8);
            run8.Append(text8);

            paragraph13.Append(paragraphProperties13);
            paragraph13.Append(run8);

            tableCell13.Append(tableCellProperties13);
            tableCell13.Append(paragraph13);

            TableCell tableCell14 = new TableCell();

            TableCellProperties tableCellProperties14 = new TableCellProperties();
            TableCellWidth tableCellWidth14 = new TableCellWidth(){ Width = "3600", Type = TableWidthUnitValues.Dxa };
            Shading shading14 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment14 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties14.Append(tableCellWidth14);
            tableCellProperties14.Append(shading14);
            tableCellProperties14.Append(tableCellVerticalAlignment14);

            Paragraph paragraph14 = new Paragraph(){ RsidParagraphMarkRevision = "003B7BF3", RsidParagraphAddition = "006D7C52", RsidParagraphProperties = "004E447F", RsidRunAdditionDefault = "006D7C52", ParagraphId = "06A207FD", TextId = "77777777" };

            ParagraphProperties paragraphProperties14 = new ParagraphProperties();
            Justification justification14 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties14 = new ParagraphMarkRunProperties();
            RunFonts runFonts22 = new RunFonts(){ Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" };
            FontSize fontSize22 = new FontSize(){ Val = "20" };
            FontSizeComplexScript fontSizeComplexScript22 = new FontSizeComplexScript(){ Val = "20" };

            paragraphMarkRunProperties14.Append(runFonts22);
            paragraphMarkRunProperties14.Append(fontSize22);
            paragraphMarkRunProperties14.Append(fontSizeComplexScript22);

            paragraphProperties14.Append(justification14);
            paragraphProperties14.Append(paragraphMarkRunProperties14);

            paragraph14.Append(paragraphProperties14);

            tableCell14.Append(tableCellProperties14);
            tableCell14.Append(paragraph14);

            tableRow1.Append(tableRowProperties1);
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
            return tableRow1;
        }
    }

}
