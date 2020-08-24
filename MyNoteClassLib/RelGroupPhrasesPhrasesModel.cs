using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNoteDataClassLib;
using Dapper;
using System.Data;
using System.Data.SQLite;

namespace MyNoteDataClassLib
{
    public class RelGroupPhrasesPhrase
    {
        public int RelGPPID { get; set;}
        public int RelGPPGroupPhraseID { get; set; }
        public int RelGPPPhraseID { get; set; }
        public int DefaultPhraseOptionID { get; set; }
        public int RelGPPOrder { get; set; }
        public Phrase ParentPhrase { get; set; }
        public GroupPhrase ParentGroupPhrase { get; set; }
        public PhraseOption DefaultPhraseOption { get; set; }
        public List<PhraseOption> PhraseOptions { get; set; }
    }
}
