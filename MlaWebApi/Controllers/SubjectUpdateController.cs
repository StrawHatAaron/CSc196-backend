using MlaWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.SqlServerCe;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Transactions;

namespace MlaWebApi.Controllers
{
    public class SubjectUpdateController : ApiController
    {
        public string cfmgr = ConfigurationManager.ConnectionStrings["MlaDatabase"].ConnectionString;
        SqlConnection cnn = null;

        public HttpResponseMessage PostSubjectUpdate(Subject subject)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DataSet dsData = new DataSet("subject");
                cnn = new SqlConnection(cfmgr);
                cnn.Open();

                try
                {
                    SqlCommand comm = new SqlCommand("Update subject set title = '" + subject.title + "'" + " , " + " description = '" + subject.description + "'" + " , " + " mailingAlias = '" + subject.mailingAlias + "'" + " , " + " videoEnabled = '" + subject.videoEnabled + "'" + " , " + " audioEnabled = '" + subject.audioEnabled + "'" + " where idSubject = '" + subject.idSubject + "'", cnn);

                    SqlDataAdapter sqlada = new SqlDataAdapter(comm);
                    sqlada.Fill(dsData);


                    //throw to test the transaction in the backend
                    //throw new System.ArgumentException("Testing transaction", "original");


                    var response = Request.CreateResponse<Subject>(System.Net.HttpStatusCode.Found, subject);
                    cnn.Close();
                    return response;
                }
                catch (Exception e)
                {
                    var response = Request.CreateResponse<Subject>(System.Net.HttpStatusCode.BadRequest, subject);
                    cnn.Close();
                    return response;
                }
            }
        } 
    }
}
