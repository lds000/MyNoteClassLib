using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;
using System.Data;


namespace MyNoteDataClassLib
{
    public class Phrase
    {
        public int PhraseID { get; set; }
        public string PhraseTitle { get; set; }

        public PhraseOption GetDefaultOption(GroupPhrase gp,  Phrase p)
        {
            string sql = $"Select * from PhraseOptions po inner join RelGroupPhrasesPhrases rgpp on po.PhraseOptionID = rgpp.RelGPPDefaultOptionID where rgpp.RelGPPGroupPhraseID = {gp.GroupPhraseID} and rgpp.RelGPPPhraseID = @PhraseID limit 1;";
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SqlLiteDataAccess.SQLiteDBLocation))
            {
                var output = cnn.QueryFirst<PhraseOption>(sql, p);
                return output;
            }
        }

        public List<PhraseOption> GetOptions()
        {
            string sql = $"SELECT p.PhraseTitle, o.PhraseOptionTitle, o.PhraseOptionValue FROM RelPhraseOption r INNER JOIN Phrases p ON p.PhraseID = r.RelPhraseOptionPID INNER JOIN PhraseOptions o ON o.PhraseOptionID = r.RelPhraseOptionPOID where p.PhraseID = { PhraseID} order by r.RelPhraseOptionID;";
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SqlLiteDataAccess.SQLiteDBLocation))
            {
                var output = cnn.Query<PhraseOption>(sql);
                return output.ToList();
            }
        }

    }

}
