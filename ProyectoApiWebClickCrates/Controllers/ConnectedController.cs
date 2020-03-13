using ProyectoApiWebClickCrates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using System.Data;

namespace ProyectoApiWebClickCrates.Controllers
{
    [Authorize]
    [RoutePrefix("api/Connected")]
    public class ConnectedController : ApiController
    {
        [HttpPost]
        [Route("InsertConnected")]
        public IHttpActionResult InsertarOnline(ConnectedModels online)
        {

            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = "INSERT INTO dbo.Connected(Id,Estado,NickName,ImageUser)" +
                $" VALUES ('{online.Id}','Menu','{online.NickName}','{online.ImageUser}')";

            try
            {

                con.Execute(sql);

            }
            catch (Exception e)
            {
                return BadRequest("Error al insertar online, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateConnectedPlayer")]
        public IHttpActionResult UpdateOnline(ConnectedModels connected)
        {
            IDbConnection con = new ApplicationDbContext().Database.Connection;


            ConnectedModels infoConnectedPlayer;

            string sqlUno = $"SELECT * FROM dbo.Connected WHERE Id = '{connected.Id}'";

            try
            {
                infoConnectedPlayer = con.Query<ConnectedModels>(sqlUno).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener Usuario Online individual, " + ex.Message);
            }

            if (!string.IsNullOrEmpty(connected.Estado))
            {
                infoConnectedPlayer.Estado = connected.Estado;
            }

           

            if (!string.IsNullOrEmpty(connected.NickName))
            {
                infoConnectedPlayer.NickName = connected.NickName;
            }
            if (!string.IsNullOrEmpty(connected.ImageUser))
            {
                infoConnectedPlayer.ImageUser = connected.ImageUser;
            }


            string sql = "UPDATE dbo.Connected " +
                $"SET NickName = '{infoConnectedPlayer.NickName}', Estado = '{infoConnectedPlayer.Estado}', " +
                $"ImageUser = '{infoConnectedPlayer.ImageUser}'" +
                $" WHERE Id = '{infoConnectedPlayer.Id}'";

            try
            {
                con.Execute(sql);

            }
            catch (Exception e)
            {
                return BadRequest("Error Update Online in database, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetConnected")]
        public List<ConnectedModels> GetOnlines()
        {
            List<ConnectedModels> lista;
            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = "SELECT * FROM dbo.Connected";

            try
            {
                lista = con.Query<ConnectedModels>(sql).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error al dar todos los usuarios online, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return lista;

        }

        [HttpPost]
        [Route("DeleteConnected")]
        public IHttpActionResult EliminarOnline(ConnectedModels connected)
        {
            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = "DELETE FROM dbo.Connected" +
                $" WHERE Id = '{connected.Id}'";

            try
            {

                con.Execute(sql);

            }
            catch (Exception e)
            {
                return BadRequest("Error al eliminar online, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();
        }
    }
}
