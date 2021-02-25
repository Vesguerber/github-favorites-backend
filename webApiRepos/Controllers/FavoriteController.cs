using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace webApiRepos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader rdr;
        const string connStr = "server=localhost;user=root;database=git-favorites;port=3306;password=anderson123";
        
        public void InitializeBD()
        {
            conn = new MySqlConnection(connStr);
            try
            {
                Debug.Write("Connecting to MySQL...");
                conn.Open();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
            Console.WriteLine("Done.");
        }

        [HttpGet]
        public IActionResult Get()
        {

            InitializeBD();


            string sql = "SELECT * FROM repoids";
            cmd = new MySqlCommand(sql, conn);
            rdr = cmd.ExecuteReader();

            List<RepoID> repos = new List<RepoID>();

            while (rdr.Read())
            {
                repos.Add(new RepoID((int)rdr[0], (int)rdr[1]));
            }
            rdr.Close();

            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin","*");

            return Ok(JsonConvert.SerializeObject(repos));
        }

        [HttpGet ("{repoid}")]
        public IActionResult Get(int repoid)
        {

            InitializeBD();



            string sql = "SELECT * FROM repoids WHERE repoid = " + repoid;
            cmd = new MySqlCommand(sql, conn);
            rdr = cmd.ExecuteReader();

            List<RepoID> repos = new List<RepoID>();
            int count = 0;
            
            while (rdr.Read())
            {
                count++;
            }
            rdr.Close();

            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            

            if (count > 0)
                return Ok(true);
            else
                return Ok(false);

        }

        [HttpPost]
    
        public IActionResult Post(JObject payload)
        {
            InitializeBD();
            string sql = "INSERT into repoids (repoId) values (" + payload.GetValue("repoId") + ")";
            cmd = new MySqlCommand(sql, conn);
            int res = cmd.ExecuteNonQuery();
            return Ok(res);
        }
    }
}

public class RepoID
{

    public int id { get; set; }
    public int repoId { get; set; }

    public RepoID(int id, int repoId)
    {
        this.id = id;
        this.repoId = repoId;
    }

    public string ToJSON()
    {
        return "x"; // JsonSerializer.Serialize<RepoID>(this);
    }

     public RepoID FromJSON(string json)
    {
        return null; // JsonSerializer.Deserialize<RepoID>(json);
    }

    public override string ToString()
    {
        return "id: " + id; 
    }

}