using Invoicing.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Invoicing.Server.Models
{
    public class BusinessDataSql : IBusinessData, IDisposable
    {
        private SqlConnection cnct;
        public BusinessDataSql(string connectionString)
        {
            cnct = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            cnct.Dispose();
        }

        public IEnumerable<Invoice> AllInvoices
            => cnct.Query<Invoice>("SELECT Client AS Customer, Numero AS Reference, Datereglement AS Created, MontantDu AS Amount, MontantRegle AS Paid FROM Factures ORDER BY Datereglement DESC");


        public void EnvoyerCA()
        {
            double ChiffreAffaire = cnct.QuerySingle<double>("SELECT SUM(MontantRegle) FROM Factures");

            var p = new DynamicParameters();
            p.Add("@CA", ChiffreAffaire, DbType.Double, ParameterDirection.Input);


            String Numero = cnct.QueryFirst<String>("SELECT Numero FROM Factures");
            String Client = cnct.QueryFirst<String>("SELECT Client FROM Factures");
            DateTime Datereglement = cnct.QueryFirst<DateTime>("SELECT Datereglement FROM Factures");
            double MontantDu = cnct.QueryFirst<double>("SELECT MontantDu FROM Factures");
            double MontantRegle = cnct.QueryFirst<double>("SELECT MontantRegle FROM Factures");




            p.Add("@Numero", Numero, DbType.String, System.Data.ParameterDirection.Input);
            p.Add("@Client", Client, DbType.String, System.Data.ParameterDirection.Input);
            p.Add("@Datereglement", Datereglement, DbType.DateTime, System.Data.ParameterDirection.Input);
            p.Add("@MontantDu", MontantDu, DbType.Double, System.Data.ParameterDirection.Input);
            p.Add("@MontantRegle", MontantRegle, DbType.Double, System.Data.ParameterDirection.Input);

            cnct.Execute(@"INSERT INTO Factures (Numero, Client, Datereglement, MontantDu, MontantRegle, CA) VALUES (@Numero, @Client, @Datereglement, @MontantDu, @MontantRegle, @CA)", p);

        }


        public double SalesRevenue => throw new NotImplementedException();

        public double Outstanding => throw new NotImplementedException();

    }
}
