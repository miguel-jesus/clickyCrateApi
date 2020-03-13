using Dapper;
using ProyectoApiWebClickCrates.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProyectoApiWebClickCrates.Controllers
{
    [Authorize]
    [RoutePrefix("api/Player")]

    public class PlayerController : ApiController
    {
        
        [HttpPost]
        [Route("InsertNewPlayer")]
        public IHttpActionResult InsertNewPlayer(PlayerModels player)
        {

            IDbConnection con = new ApplicationDbContext().Database.Connection;

           

            string sql = "INSERT INTO dbo.Player (Id, FirstName, LastName, NickName, Email, BirthDate, City, BlobUri) " +
                    $"VALUES ('{player.Id}','{player.FirstName}','{player.LastName}','{player.NickName}','{player.Email}'," +
                    $"CONVERT(datetime2,'{player.BirthDate}'),'{player.City}','{player.BlobUri}')";

            try
            {
                con.Execute(sql);

            }
            catch (Exception e)
            {
                return BadRequest("Error inserting player in database, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();

        }

        
        [HttpGet]
        [Route("GetPlayer/{id}")]
        public PlayerModels GetPlayer(string Id)
        {
            PlayerModels player;

            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = $"SELECT * FROM dbo.Player WHERE id = '{Id}'";

            try
            {

                player = con.Query<PlayerModels>(sql).FirstOrDefault();



            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return player;

        }

        [HttpPost]
        [Route("UpdatePlayer")]
        public IHttpActionResult UpdatePlayer(PlayerModels player)
        {

            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = "UPDATE dbo.Players " +
                $"SET FirstName = '{player.FirstName}', LastName = '{player.LastName}', NickName = '{player.NickName}'" +
                $"WHERE Id = '{player.Id}'";

            try
            {
                con.Execute(sql);

            }
            catch (Exception e)
            {
                return BadRequest("Error Update player in database, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();
        }

    }
}
