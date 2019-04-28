using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrgChartDemo.Models.DocGenerators
{
    public class AlphaRosterGenerator
    {
        public List<Member> Members { get; set; }
        public string ComponentName { get; set; }
        private List<MappedField> Fields { get; set; }
        private List<string> TitleColumns { get; set; }
        public AlphaRosterGenerator()
        {
        }

        public MemoryStream Generate()
        {
            var mem = new MemoryStream();
            byte[] byteArray = File.ReadAllBytes("Templates/Alpha_Roster_Template.docx");
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(mem, true))
            {
                // init Mapped Fields Collection
                InitializeFieldMap(wordDoc);
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                // Write the Static fields
                MappedField componentName = Fields.First(f => f.FieldName == "MainComponentName");
                componentName.Write(ComponentName);
                MappedField rosterDate = Fields.First(f => f.FieldName == "RosterDate");
                rosterDate.Write(DateTime.Today.ToShortDateString());
                Table rosterTable = mainPart.Document.Body.Elements<Table>().ElementAt(0);
                foreach (Member m in Members.OrderBy(x => x.LastName))
                {
                    TableRow tr = new TableRow();
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(m.Position.Name)))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(m.GetLastNameFirstName())))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(m.Rank.RankFullName)))));
                    // we have to build the paragraph to set the justification inside the cell for these values
                    Paragraph p1 = new Paragraph();
                    ParagraphProperties pp1 = new ParagraphProperties();
                    Justification pj1 = new Justification(){ Val = JustificationValues.Center };
                    pp1.Append(pj1);
                    p1.Append(pp1);
                    p1.Append(new Run(new Text($"{m.Race.Abbreviation}/{m.Gender.Abbreviation}")));
                    tr.Append(new TableCell(p1));
                    // same here... I want this cell to be centered
                    Paragraph p2 = new Paragraph();
                    // I learned the hard way that I need to append the centering Justification to the paragraph
                    // BEFORE the run, or it won't center.
                    ParagraphProperties pp2 = new ParagraphProperties();
                    Justification pj2 = new Justification(){ Val = JustificationValues.Center };
                    pp2.Append(pj2);
                    p2.Append(pp2);
                    p2.Append(new Run(new Text($"#{m.IdNumber}")));
                    tr.Append(new TableCell(p2));
                    rosterTable.Append(tr);
                }

                Table demoTable = mainPart.Document.Body.Elements<Table>().ElementAt(1);
                List<Rank> distinctRankList = Members.Select(x => x.Rank).OrderByDescending(x => x.RankId).Distinct().ToList();
                
                foreach (Rank r in distinctRankList)
                {
                    List<Member> RankMembers = Members.Where(x => x.Rank == r).ToList();
                    demoTable.Append(GenerateDemoTableRow(r.RankFullName, RankMembers));
                }
                List<Member> SwornMembers = Members.Where(x => x.Rank.IsSworn).ToList();
                demoTable.Append(GenerateDemoTableRow("Sworn", SwornMembers));
                List<Member> CivilianMembers = Members.Where(x => x.Rank.IsSworn == false).ToList();
                demoTable.Append(GenerateDemoTableRow("Civilian", CivilianMembers));
                demoTable.Append(GenerateDemoTableRow("Total", Members));
                
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }     

        private void InitializeFieldMap(WordprocessingDocument _doc)
        {
            MainDocumentPart mainPart = _doc.MainDocumentPart;
            Table rosterTable = mainPart.Document.Body.Elements<Table>().ElementAt(0);
            Table demoTable = mainPart.Document.Body.Elements<Table>().ElementAt(1);
            Fields = new List<MappedField>
            {
                new MappedField { FieldName = "MainComponentName", RowIndex = 0, CellIndex = 0, TableIndex = 0, Table = rosterTable },
                new MappedField { FieldName = "RosterDate", RowIndex = 1, CellIndex = 0, TableIndex = 0, Table = rosterTable },
                new MappedField { FieldName = "Assignment", RowIndex = 3, CellIndex = 0, TableIndex = 0, Table = rosterTable },
                new MappedField { FieldName = "MemberName", RowIndex = 3, CellIndex = 1, TableIndex = 0, Table = rosterTable },
                new MappedField { FieldName = "MemberRank", RowIndex = 3, CellIndex = 2, TableIndex = 0, Table = rosterTable },
                new MappedField { FieldName = "MemberRaceGender", RowIndex = 3, CellIndex = 3, TableIndex = 0, Table = rosterTable },
                new MappedField { FieldName = "MemberBadgeNumber", RowIndex = 3, CellIndex = 0, TableIndex = 0, Table = rosterTable },
            };          
        }

        private TableRow GenerateDemoTableRow(string ColumnName, List<Member> MemberList)
        {
             TableRow tr = new TableRow();
            // append the "Rank Name" (First Column) cell
            Paragraph p0 = new Paragraph();
            ParagraphProperties pp0 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines0 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            pp0.Append(spacingBetweenLines0);
            
            p0.Append(pp0);
            Run tr0 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp0 = new RunProperties(new Bold());
                tr0.Append(rp0);
            }            
            tr0.Append(new Text(ColumnName));
            p0.Append(tr0);
            TableCell tc0 = new TableCell();
            TableCellProperties tcp0 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs0 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp0.Append(tcs0);
            }
            tc0.Append(tcp0);
            tc0.Append(p0);
            tr.Append(tc0);

            // B/M column (this needs a 1.5pt left border)
            // set properties to apply to all text centering
            int BMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Black" && x.Gender.GenderFullName == "Male");
            Run tr1 = new Run();                    
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp1 = new RunProperties(new Bold());
                tr1.Append(rp1);
            }
            tr1.Append(new Text(BMCount != 0 ? $"{BMCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp1 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp1.Append(spacingBetweenLines1);
            Paragraph p1 = new Paragraph(pp1);
            p1.Append(tr1);

            TableCellProperties tcp1 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs1 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp1.Append(tcs1);
            }
            TableCellBorders tcb1 = new TableCellBorders(new LeftBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp1.Append(tcb1);
            TableCell tc1 = new TableCell(tcp1);
                    
            tc1.Append(p1);
            tr.Append(tc1);

            // H/M column
            int HMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Hispanic" && x.Gender.GenderFullName == "Male");
            Run tr2 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp2 = new RunProperties(new Bold());
                tr2.Append(rp2);
            }
            tr2.Append(new Text(HMCount != 0 ? $"{HMCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp2 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp2.Append(spacingBetweenLines2);

            Paragraph p2 = new Paragraph(pp2);
            p2.Append(tr2);
                    
            TableCellProperties tcp2 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs2 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp2.Append(tcs2);
            }
            TableCell tc2 = new TableCell(tcp2);                                        
                    
            tc2.Append(p2);                    
            tr.Append(tc2);

            // A/M column
            int AMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Asian" && x.Gender.GenderFullName == "Male");
            Run tr3 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp3 = new RunProperties(new Bold());
                tr3.Append(rp3);
            }
            tr3.Append(new Text(AMCount != 0 ? $"{AMCount}" : "-"));
            
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp3 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp3.Append(spacingBetweenLines3);

            Paragraph p3 = new Paragraph(pp3);                                        
            p3.Append(tr3);

                    
            TableCellProperties tcp3 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });                                       
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs3 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp3.Append(tcs3);
            }
            
            TableCell tc3 = new TableCell(tcp3);
                    
            tc3.Append(p3);                    
            tr.Append(tc3);

            // W/M Column
            int WMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "White" && x.Gender.GenderFullName == "Male");
            Run tr4 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp4 = new RunProperties(new Bold());
                tr4.Append(rp4);
            }        
            tr4.Append(new Text(WMCount != 0 ? $"{WMCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp4 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp4.Append(spacingBetweenLines4);

            Paragraph p4 = new Paragraph(pp4);
            p4.Append(tr4);
                                        
            TableCellProperties tcp4 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs4 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp4.Append(tcs4);
            }
            TableCellBorders tcb4 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp4.Append(tcb4);

            TableCell tc4 = new TableCell(tcp4);                    
            tc4.Append(p4);
            tr.Append(tc4);

            // B/F Column
            int BFCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Black" && x.Gender.GenderFullName == "Female");
            Run tr5 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp5 = new RunProperties(new Bold());
                tr5.Append(rp5);
            }
            tr5.Append(new Text(BFCount != 0 ? $"{BFCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp5 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp5.Append(spacingBetweenLines5);

            Paragraph p5 = new Paragraph(pp5);
            p5.Append(tr5);
                    
            TableCellProperties tcp5 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs5 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp5.Append(tcs5);
            }
            TableCell tc5 = new TableCell(tcp5);                                        
                    
            tc5.Append(p5);                    
            tr.Append(tc5);
                    
            // H/F column
            int HFCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Hispanic" && x.Gender.GenderFullName == "Female");
            Run tr6 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp6 = new RunProperties(new Bold());
                tr6.Append(rp6);
            }
            tr6.Append(new Text(HFCount != 0 ? $"{HFCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp6 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp6.Append(spacingBetweenLines6);

            Paragraph p6 = new Paragraph(pp6);
            p6.Append(tr6);
                    
            TableCellProperties tcp6 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs6 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp6.Append(tcs6);
            }
            TableCell tc6 = new TableCell(tcp6);                                        
                    
            tc6.Append(p6);                    
            tr.Append(tc6);
                    
            // A/F column
            int AFCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Asian" && x.Gender.GenderFullName == "Female");
            Run tr7 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp7 = new RunProperties(new Bold());
                tr7.Append(rp7);
            }
            tr7.Append(new Text(AFCount != 0 ? $"{AFCount}" : "-"));

            SpacingBetweenLines spacingBetweenLines7 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp7 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp7.Append(spacingBetweenLines7);

            Paragraph p7 = new Paragraph(pp7);
            p7.Append(tr7);
                    
            TableCellProperties tcp7 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs7 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp7.Append(tcs7);
            }
            TableCell tc7 = new TableCell(tcp7);                                        
                    
            tc7.Append(p7);                    
            tr.Append(tc7);

            // W/F Column
            int WFCount =  MemberList.Count(x => x.Race.MemberRaceFullName == "White" && x.Gender.GenderFullName == "Female");
            Run tr8 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp8 = new RunProperties(new Bold());
                tr8.Append(rp8);
            } 
            tr8.Append(new Text(WFCount != 0 ? $"{WFCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines8 = new SpacingBetweenLines(){ After = "0", Line = "280", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp8 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp8.Append(spacingBetweenLines8);

            Paragraph p8 = new Paragraph(pp8);
            p8.Append(tr8);
                                        
            TableCellProperties tcp8 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs8 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp8.Append(tcs8);
            }
            TableCellBorders tcb8 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp8.Append(tcb8);

            TableCell tc8 = new TableCell(tcp8);                    
            tc8.Append(p8);
            tr.Append(tc8);

            // B/T Column
            int BTCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Black");
            Run tr9 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp9 = new RunProperties(new Bold());
                tr9.Append(rp9);
            } 
            tr9.Append(new Text(BTCount != 0 ? $"{BTCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines9 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp9 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp9.Append(spacingBetweenLines9);

            Paragraph p9 = new Paragraph(pp9);
            p9.Append(tr9);
                    
            TableCellProperties tcp9 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs9 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp9.Append(tcs9);
            }
            TableCell tc9 = new TableCell(tcp9);                                        
                    
            tc9.Append(p9);                    
            tr.Append(tc9);

            // H/T Column
            int HTCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Hispanic");
            Run tr10 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp10 = new RunProperties(new Bold());
                tr10.Append(rp10);
            } 
            tr10.Append(new Text(HTCount != 0 ? $"{HTCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp10 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp10.Append(spacingBetweenLines10);

            Paragraph p10 = new Paragraph(pp10);
            p10.Append(tr10);
                    
            TableCellProperties tcp10 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs10 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp10.Append(tcs10);
            }
            TableCell tc10 = new TableCell(tcp10);                                        
                    
            tc10.Append(p10);                    
            tr.Append(tc10);

            // A/T Column
            int ATCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Asian");
            Run tr11 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp11 = new RunProperties(new Bold());
                tr11.Append(rp11);
            } 
            tr11.Append(new Text(ATCount != 0 ? $"{ATCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines11 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp11 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp11.Append(spacingBetweenLines11);

            Paragraph p11 = new Paragraph(pp11);
            p11.Append(tr11);
                    
            TableCellProperties tcp11 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs11 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp11.Append(tcs11);
            }
            TableCell tc11 = new TableCell(tcp11);                                        
                    
            tc11.Append(p11);                    
            tr.Append(tc11);

            // W/T Column
            int WTCount = MemberList.Count(x => x.Race.MemberRaceFullName == "White");
            Run tr12 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp12 = new RunProperties(new Bold());
                tr12.Append(rp12);
            }         
            tr12.Append(new Text(WTCount != 0 ? $"{WTCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines12 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp12 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp12.Append(spacingBetweenLines12);

            Paragraph p12 = new Paragraph(pp12);
            p12.Append(tr12);
                                        
            TableCellProperties tcp12 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs12 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp12.Append(tcs12);
            }
            TableCellBorders tcb12 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp12.Append(tcb12);

            TableCell tc12 = new TableCell(tcp12);                    
            tc12.Append(p12);
            tr.Append(tc12);

            // TotalForRank column
            int RankTotalCount = MemberList.Count;
            Run tr13 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp13 = new RunProperties(new Bold());
                tr13.Append(rp13);
            }  
            tr13.Append(new Text(RankTotalCount != 0 ? $"{RankTotalCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines13 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp13 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp13.Append(spacingBetweenLines13);

            Paragraph p13 = new Paragraph(pp13);
            p13.Append(tr13);
                                        
            TableCellProperties tcp13 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs13 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp13.Append(tcs13);
            }
            TableCellBorders tcb13 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)13U, Space = (UInt32Value)0U });
            tcp13.Append(tcb13);

            TableCell tc13 = new TableCell(tcp13);                    
            tc13.Append(p13);
            tr.Append(tc13);
            return tr;
        }
    }
}
