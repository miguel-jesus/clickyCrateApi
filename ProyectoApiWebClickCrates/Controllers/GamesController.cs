using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using ProyectoApiWebClickCrates.Models;

namespace ProyectoApiWebClickCrates.Controllers
{
    [Authorize]
    [RoutePrefix("api/Games")]
    public class GamesController : ApiController
    {

        [HttpPost]
        [Route("InsertNewGame")]

        public IHttpActionResult InsertPartidaNueva(GamesModels partida)
        {

            IDbConnection con = new ApplicationDbContext().Database.Connection;

            // IdJugador,Inicio, Final, Dificultad y puntuacion
            string sql = "INSERT INTO dbo.Games(IdPlayer,Final,Difficulty,Points)" +
                $" VALUES ('{partida.IdPlayer}',null,{partida.Difficulty},0)";

            try
            {
                con.Execute(sql);
            }
            catch (Exception e)
            {
                return BadRequest("Error insertar nueva partida," + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();

        }


        [HttpPost]
        [Route("UpdateGame")]
        public IHttpActionResult UpdatePartida(GamesModels games)
        {



            GamesModels newGame;
            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sqlIdPartida = $"SELECT Id FROM dbo.Games WHERE IdPlayer = '{games.IdPlayer}' ORDER BY Id ASC";

            try
            {
                newGame = con.Query<GamesModels>(sqlIdPartida).LastOrDefault();
            }
            catch (Exception ex)
            {
                return BadRequest("No se ha podido encontrar la partida, " + ex.Message);
            }

            string sql = "UPDATE dbo.Games " +
                $"SET Final = '{DateTime.Now}', Points = {games.Points}" +
                $" WHERE Id = '{newGame.Id}'";

            try
            {
                con.Execute(sql);
            }
            catch (Exception e)
            {
                return BadRequest("Error al actualizar una partida," + e.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetGames/{Id}")]
        public List<GamesModels> GetPartidas(string Id)
        {
            List<GamesModels> partidas;
            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = $"SELECT * FROM dbo.Games WHERE IdPlayer = '{Id}' ORDER BY Points DESC";

            try
            {
                partidas = con.Query<GamesModels>(sql).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error al devolver todas las partidas de un jugador, " + e.Message);
            }
            finally
            {
                con.Close();
            }

            return partidas;
        }

    }
}
