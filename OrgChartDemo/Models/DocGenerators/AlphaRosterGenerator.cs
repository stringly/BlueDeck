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
        private Dictionary<string, Dictionary<string, int>> totalCollection { get; set; }
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
                PopulateDemographics();
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
                foreach (KeyValuePair<string, Dictionary<string, int>> entry in totalCollection)
                {
                    TableRow tr = new TableRow();
                    // append the "Rank Name" (First Column) cell
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Key.ToString())))));

                    // B/M column (this needs a 1.5pt left border)
                    // set properties to apply to all text centering
                    ParagraphProperties pp1 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
                    TableCellProperties tcp1 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
                    TableCellBorders tcb1 = new TableCellBorders();
                    LeftBorder lb1 = new LeftBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                    tcb1.Append(lb1);

                    TableCell tc1 = new TableCell();
                    tc1.Append(tcp1);
                    tc1.Append(tcb1);

                    Paragraph p1 = new Paragraph();
                    p1.Append(pp1);                    
                    p1.Append(new Run(new Text(entry.Value["BM"] != 0 ? $"{entry.Value["BM"]}" : "")));                    

                    tc1.Append(p1);
                    tr.Append(tc1);
                    // H/M column
                    ParagraphProperties pp2 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
                    TableCellProperties tcp2 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
                    TableCell tc2 = new TableCell(tcp2);
                    Paragraph p2 = new Paragraph(pp2);
                    p2.Append(new Run(new Text(entry.Value["HM"] != 0 ? $"{entry.Value["HM"]}" : "")));
                    tc2.Append(p2);                    
                    tr.Append(tc2);

                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["AM"] != 0 ? $"{entry.Value["AM"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["WM"] != 0 ? $"{entry.Value["WM"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["BF"] != 0 ? $"{entry.Value["BF"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["HF"] != 0 ? $"{entry.Value["HF"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["AF"] != 0 ? $"{entry.Value["AF"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["WF"] != 0 ? $"{entry.Value["WF"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["BT"] != 0 ? $"{entry.Value["BT"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["HT"] != 0 ? $"{entry.Value["HT"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["AT"] != 0 ? $"{entry.Value["AT"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["WT"] != 0 ? $"{entry.Value["WT"]}" : "")))));
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(entry.Value["TotalForRank"] == 0 ? $"{entry.Value["TotalForRank"]}" : "")))));
                    demoTable.Append(tr);
                }
                
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }
        
        private void PopulateDemographics()
        {
            // first, I need to know how many Ranks exist in the Members Collection, as this determines
            // the number of rows.
            List<string> ranks = Members.Select(x => x.Rank.RankFullName).Distinct().ToList();
            totalCollection = new Dictionary<string, Dictionary<string, int>>();
            totalCollection.Add("Sworn", new Dictionary<string, int>()
                {
                    { "BM", 0 },
                    { "HM", 0 },
                    { "AM", 0 },
                    { "WM", 0 },
                    { "BF", 0 },
                    { "HF", 0 },
                    { "AF", 0 },
                    { "WF", 0 },
                    { "BT", 0 },
                    { "HT", 0 },
                    { "AT", 0 },
                    { "WT", 0 },
                    { "TotalForRank", 0 }
                });
            totalCollection.Add("Civilian", new Dictionary<string, int>()
                {
                    { "BM", 0 },
                    { "HM", 0 },
                    { "AM", 0 },
                    { "WM", 0 },
                    { "BF", 0 },
                    { "HF", 0 },
                    { "AF", 0 },
                    { "WF", 0 },
                    { "BT", 0 },
                    { "HT", 0 },
                    { "AT", 0 },
                    { "WT", 0 },
                    { "TotalForRank", 0 }
                });
            totalCollection.Add("Station", new Dictionary<string, int>()
                {
                    { "BM", 0 },
                    { "HM", 0 },
                    { "AM", 0 },
                    { "WM", 0 },
                    { "BF", 0 },
                    { "HF", 0 },
                    { "AF", 0 },
                    { "WF", 0 },
                    { "BT", 0 },
                    { "HT", 0 },
                    { "AT", 0 },
                    { "WT", 0 },
                    { "TotalForRank", 0 }
                });
            foreach (string rank in ranks)
            {                
                Dictionary<string, int> demo = new Dictionary<string, int>()
                {
                    { "BM", 0 },
                    { "HM", 0 },
                    { "AM", 0 },
                    { "WM", 0 },
                    { "BF", 0 },
                    { "HF", 0 },
                    { "AF", 0 },
                    { "WF", 0 },
                    { "BT", 0 },
                    { "HT", 0 },
                    { "AT", 0 },
                    { "WT", 0 },
                    { "TotalForRank", 0 }
                };
                List<Member> rankMembers = Members.Where(x => x.Rank.RankFullName == rank).ToList();
                foreach (Member m in rankMembers)
                {
                    if (m.Race.MemberRaceFullName != "Other" && m.Gender.GenderFullName != "Unknown") { 
                        demo[$"{m.Race.Abbreviation}{m.Gender.Abbreviation}"]++;
                        demo[$"{m.Race.Abbreviation}T"]++;
                        demo["TotalForRank"]++;
                        if (m.Rank.IsSworn)
                        {
                            totalCollection["Sworn"][$"{m.Race.Abbreviation}{m.Gender.Abbreviation}"]++;
                            totalCollection["Sworn"][$"{m.Race.Abbreviation}T"]++;
                            totalCollection["Sworn"]["TotalForRank"]++;
                        }
                        else
                        {
                            totalCollection["Civilian"][$"{m.Race.Abbreviation}{m.Gender.Abbreviation}"]++;
                            totalCollection["Civilian"][$"{m.Race.Abbreviation}T"]++;
                            totalCollection["Sworn"]["TotalForRank"]++;
                        }
                        totalCollection["Station"][$"{m.Race.Abbreviation}{m.Gender.Abbreviation}"]++;
                        totalCollection["Station"][$"{m.Race.Abbreviation}T"]++;
                        totalCollection["Station"]["TotalForRank"]++;
                        
                    }
                }
                totalCollection.Add(rank, demo);

            }            
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
            Dictionary<string, int> Ranks = new Dictionary<string, int>
            {
                { "Major", 2 },
                { "Captain", 3 },
                { "Lieutenant", 4 },
                { "Sergeant", 5 },
                { "Corporal", 6 },
                { "Police Officer First Class", 7 },
                { "Police Officer", 8 },
                { "Civilian", 10 }
            };
            foreach(KeyValuePair<string, int> rank in Ranks)
            {
                for (int i = 1; i > 14; i++)
                {
                    string gender = "";
                    string race = "";
                    if (i < 5 )
                    {
                        gender = "_Male";
                    }
                    else if (i >= 5 && i < 9)
                    {
                        gender = "_Female";
                    }
                    else
                    {
                        gender = "_Totals";
                    }

                    if (i == 1 || i == 5 || i == 10)
                    {
                        race = "_Black";
                    }
                    if (i == 2 || i == 6 || i == 11)
                    {
                        race = "_Hispanic";
                    }
                    if (i == 5 || i == 7 || i == 12)
                    {
                        race = "_Asian";
                    }
                    if (i == 6 || i == 8 || i == 13)
                    {
                        race = "_White";
                    }

                    MappedField x = new MappedField { FieldName = $"{rank.Key}{gender}{race}", CellIndex = i, RowIndex = rank.Value, Table = demoTable, TableIndex = 1 };
                    Fields.Add(x);
                }
            }
            


        }


    }
}
