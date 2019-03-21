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
        public List<MappedField> Fields { get; set; }

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
                InitializeFieldMap(wordDoc);
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                MappedField x = Fields.First(f => f.FieldName == "MainComponentName");
                x.Write("TEST COMPONENT");
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
            Dictionary<string, int> Ranks = new Dictionary<string, int>
            {
                { "Major", 2 },
                { "Captain", 3 },
                { "Lieutenant", 4 },
                { "Sergeant", 5 },
                { "Corporal", 6 },
                { "POFC", 7 },
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
