using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace proyectoCine
{
    class DAOCine
    {
        private SqliteConnection Conexion;
        private readonly string crearTablaPeliculas = @"CREATE TABLE IF NOT EXISTS peliculas(
                                                idPelicula      INTEGER PRIMARY KEY,
                                                titulo          TEXT, 
                                                cartel          TEXT, 
                                                año             INTEGER, 
                                                genero          TEXT, 
                                                calificacion    TEXT)";
        private readonly string crearTablaSalas = @"CREATE TABLE IF NOT EXISTS salas(
                                            idSala      INTEGER PRIMARY KEY AUTOINCREMENT,
                                            numero      TEXT, 
                                            capacidad   INTEGER,
                                            disponible  BOOLEAN DEFAULT (true))";
        private readonly string crearTablaSesiones = @"CREATE TABLE IF NOT EXISTS sesiones(
                                            idSesion INTEGER PRIMARY KEY AUTOINCREMENT,
                                            pelicula INTEGER REFERENCES peliculas (idPelicula),
                                            sala     INTEGER REFERENCES salas (idSala),
                                            hora     TEXT)";
        private readonly string crearTablaVentas = @"CREATE TABLE IF NOT EXISTS ventas(
                                            idVenta  INTEGER PRIMARY KEY AUTOINCREMENT,
                                            sesion   INTEGER REFERENCES sesiones (idSesion),
                                            cantidad INTEGER,
                                            pago     TEXT)";

        public DAOCine()
        {
            Conexion = new SqliteConnection("Data source=cine");
            CrearTablas();
        }

        private void CrearTablas()
        {
            Conexion.Open();
            SqliteCommand comando;
            comando = Conexion.CreateCommand();

            comando.CommandText = crearTablaPeliculas;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaSalas;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaSesiones;
            comando.ExecuteNonQuery();
            comando.CommandText = crearTablaVentas;
            comando.ExecuteNonQuery();

            Conexion.Close();
        }

        public ObservableCollection<Pelicula> ObtenPeliculas()
        {
            ObservableCollection<Pelicula> peliculas = new ObservableCollection<Pelicula>();
            SqliteDataReader datos = null;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT * FROM  peliculas";
                datos = comando.ExecuteReader();

                if (datos.HasRows)
                {
                    string titulo, cartel, genero, calificacion;
                    int id, año;
                    while (datos.Read())
                    {
                        id = datos.GetInt32(datos.GetOrdinal("idPelicula"));
                        titulo = (string)datos["titulo"];
                        cartel = (string)datos["cartel"];
                        año = datos.GetInt32(datos.GetOrdinal("año"));
                        genero = (string)datos["genero"];
                        calificacion = (string)datos["calificacion"];
                        peliculas.Add(new Pelicula(id, titulo, cartel, año, genero, calificacion));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Leer películas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (datos != null && !datos.IsClosed)
                        datos.Close();
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return peliculas;
        }

        public ObservableCollection<Salas> ObtenSalas()
        {
            ObservableCollection<Salas> salas = new ObservableCollection<Salas>();
            SqliteDataReader datos = null;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT * FROM  salas";
                datos = comando.ExecuteReader();

                if (datos.HasRows)
                {
                    int id, capacidad;
                    string numero;
                    bool disponible;
                    while (datos.Read())
                    {
                        id = datos.GetInt32(datos.GetOrdinal("idSala"));
                        capacidad = datos.GetInt32(datos.GetOrdinal("capacidad"));
                        numero = (string)datos["numero"];
                        disponible = datos.GetBoolean(datos.GetOrdinal("disponible"));

                        salas.Add(new Salas(id, disponible, capacidad, numero));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Leer salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (datos != null && !datos.IsClosed)
                        datos.Close();
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return salas;
        }

        public ObservableCollection<Sesion> ObtenSesiones()
        {
            ObservableCollection<Sesion> sesiones = new ObservableCollection<Sesion>();
            SqliteDataReader datos = null;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT idSesion, " +
                        "peliculas.idPelicula AS 'idPelicula', " +
                        "peliculas.titulo AS 'titulo'," +
                        "peliculas.cartel AS 'cartel'," +
                        "peliculas.genero AS 'genero'," +
                        "peliculas.año AS 'año'," +
                        "peliculas.calificacion AS 'calificacion'," +
                        "salas.idSala AS 'idSala'," +
                        "salas.numero AS 'nombreSala'," +
                        "salas.capacidad AS 'capacidad'," +
                        "salas.disponible AS 'disponible'," +
                        "hora " +
                    "FROM    peliculas JOIN sesiones JOIN salas " +
                        "ON  sesiones.pelicula = peliculas.idPelicula " +
                            "AND sesiones.sala = salas.idSala; ";
                datos = comando.ExecuteReader();

                if (datos.HasRows)
                {
                    int idSesion, idPelicula, idSala, capacidad, año;
                    string hora, tituloPelicula, cartel, genero, nombreSala, calificacion;
                    bool disponible;

                    while (datos.Read())
                    {

                        idPelicula = datos.GetInt32(datos.GetOrdinal("idPelicula"));
                        tituloPelicula = (string)datos["titulo"];
                        cartel = (string)datos["cartel"];
                        genero = (string)datos["genero"];
                        año = datos.GetInt32(datos.GetOrdinal("año"));
                        calificacion = (string)datos["calificacion"];
                        Pelicula p = new Pelicula(idPelicula, tituloPelicula, cartel, año, genero, calificacion);

                        idSala = datos.GetInt32(datos.GetOrdinal("idSala"));
                        nombreSala = (string)datos["nombreSala"];
                        capacidad = datos.GetInt32(datos.GetOrdinal("capacidad"));
                        disponible = datos.GetInt32(datos.GetOrdinal("disponible")) != 0;
                        Salas s = new Salas(idSala, disponible, capacidad, nombreSala);

                        idSesion = datos.GetInt32(datos.GetOrdinal("idSesion"));
                        hora = (string)datos["hora"];


                        sesiones.Add(new Sesion(idSesion, p, s, hora));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Leer salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (datos != null && !datos.IsClosed)
                        datos.Close();
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return sesiones;
        }

        public Sesion ObtenSesion(int idSesion)
        {
            Sesion sesion = null;
            SqliteDataReader datos = null;
            try
            {
                SqliteCommand comando;

                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT idSesion, " +
                                        "peliculas.idPelicula AS 'idPelicula', " +
                                        "peliculas.titulo AS 'titulo'," +
                                        "peliculas.cartel AS 'cartel'," +
                                        "peliculas.genero AS 'genero'," +
                                        "peliculas.año AS 'año'," +
                                        "peliculas.calificacion AS 'calificacion'," +
                                        "salas.idSala AS 'idSala'," +
                                        "salas.numero AS 'nombreSala'," +
                                        "salas.capacidad AS 'capacidad'," +
                                        "salas.disponible AS 'disponible'," +
                                        "hora " +
                                    "FROM    peliculas JOIN sesiones JOIN salas " +
                                        "ON  sesiones.pelicula = peliculas.idPelicula " +
                                            "AND sesiones.sala = salas.idSala " +
                                    "WHERE sesiones.IdSesion=" + idSesion;
                datos = comando.ExecuteReader();

                if (datos.HasRows)
                {
                    int idPelicula, idSala, capacidad, año;
                    string hora, tituloPelicula, cartel, genero, nombreSala, calificacion;
                    bool disponible;
                    if (datos.Read())
                    {
                        idPelicula = datos.GetInt32(datos.GetOrdinal("idPelicula"));
                        tituloPelicula = (string)datos["titulo"];
                        cartel = (string)datos["cartel"];
                        genero = (string)datos["genero"];
                        año = datos.GetInt32(datos.GetOrdinal("año"));
                        calificacion = (string)datos["calificacion"];
                        Pelicula p = new Pelicula(idPelicula, tituloPelicula, cartel, año, genero, calificacion);

                        idSala = datos.GetInt32(datos.GetOrdinal("idSala"));
                        nombreSala = (string)datos["nombreSala"];
                        capacidad = datos.GetInt32(datos.GetOrdinal("capacidad"));
                        disponible = datos.GetInt32(datos.GetOrdinal("disponible")) != 0;
                        Salas s = new Salas(idSala, disponible, capacidad, nombreSala);

                        hora = (string)datos["hora"];

                        sesion = new Sesion(idSesion, p, s, hora);
                    }
                    datos.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Leer sesion en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return sesion;
        }

        public ObservableCollection<Ventas> ObtenVentas()
        {
            ObservableCollection<Ventas> ventas = new ObservableCollection<Ventas>();
            SqliteDataReader datos = null;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT * FROM ventas";
                datos = comando.ExecuteReader();

                if (datos.HasRows)
                {
                    int idVenta, idSesion, cantidad;
                    string formaPago;

                    while (datos.Read())
                    {
                        idVenta = datos.GetInt32(datos.GetOrdinal("idVenta"));
                        idSesion = datos.GetInt32(datos.GetOrdinal("sesion"));
                        cantidad = datos.GetInt32(datos.GetOrdinal("cantidad"));
                        formaPago = (string)datos["pago"];

                        Sesion sesion = ObtenSesion(idSesion);
                        ventas.Add(new Ventas(idVenta, sesion, cantidad, formaPago));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Leer Ventas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (datos != null && !datos.IsClosed)
                        datos.Close();
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return ventas;
        }

        public ObservableCollection<OcupacionSalas> ObtenOcupacion()
        {
            ObservableCollection<OcupacionSalas> ocupacionSalas = new ObservableCollection<OcupacionSalas>();
            SqliteDataReader datos = null;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT sesiones.idSesion AS 'idSesion', SUM(ventas.cantidad) AS 'vendidas', salas.capacidad - SUM(ventas.cantidad) AS 'disponibles' " +
                                        "FROM salas JOIN sesiones JOIN ventas " +
                                            "ON salas.idSala=sesiones.sala AND sesiones.idSesion =ventas.sesion " +
                                        "GROUP BY sesiones.idSesion ;";
                datos = comando.ExecuteReader();

                if (datos.HasRows)
                {
                    int idSesion, vendidas, disponibles;

                    while (datos.Read())
                    {

                        idSesion= datos.GetInt32(datos.GetOrdinal("idSesion"));
                        vendidas= datos.GetInt32(datos.GetOrdinal("vendidas"));
                        disponibles= datos.GetInt32(datos.GetOrdinal("disponibles"));

                        Sesion sesion = ObtenSesion(idSesion);
                        ocupacionSalas.Add(new OcupacionSalas(vendidas, disponibles, sesion));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Leer salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (datos != null && !datos.IsClosed)
                        datos.Close();
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return ocupacionSalas;
        }

        public void InsertaPelicula(ObservableCollection<Pelicula> peliculas)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "DELETE FROM sesiones";
                comando.ExecuteNonQuery();

                comando.CommandText = "DELETE FROM peliculas";
                comando.ExecuteNonQuery();

                comando.CommandText = "DELETE FROM ventas";
                comando.ExecuteNonQuery();

                //resetear el AUTOINCREMENT de la tabla ventas
                comando.CommandText = "DELETE FROM sqlite_sequence WHERE name='ventas'";
                comando.ExecuteNonQuery();

                comando.Parameters.Add("@id", SqliteType.Integer);
                comando.Parameters.Add("@titulo", SqliteType.Text);
                comando.Parameters.Add("@cartel", SqliteType.Text);
                comando.Parameters.Add("@año", SqliteType.Integer);
                comando.Parameters.Add("@genero", SqliteType.Text);
                comando.Parameters.Add("@calificacion", SqliteType.Text);
                comando.CommandText = "INSERT INTO peliculas VALUES (@id,@titulo, @cartel,@año,@genero,@calificacion) ";
                foreach (Pelicula pelicula in peliculas)
                {
                    comando.Parameters["@id"].Value = pelicula.Id;
                    comando.Parameters["@titulo"].Value = pelicula.Titulo;
                    comando.Parameters["@cartel"].Value = pelicula.Cartel;
                    comando.Parameters["@año"].Value = pelicula.Año;
                    comando.Parameters["@genero"].Value = pelicula.Genero;
                    comando.Parameters["@calificacion"].Value = pelicula.Calificacion;
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertar películas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                    Conexion.Close();
            }
        }

        public void InsertaSalas(ObservableCollection<Salas> salas)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();
                comando.CommandText = "DELETE FROM salas";
                comando.ExecuteNonQuery();
                comando.Parameters.Add("@id", SqliteType.Integer);
                comando.Parameters.Add("@disponible", SqliteType.Integer);
                comando.Parameters.Add("@capacidad", SqliteType.Integer);
                comando.Parameters.Add("@numero", SqliteType.Text);
                comando.CommandText = "INSERT INTO salas VALUES(@id, @numero, @capacidad, @disponible)";
                foreach (Salas sala in salas)
                {
                    comando.Parameters["@id"].Value = sala.Id;
                    comando.Parameters["@numero"].Value = sala.Numero;
                    comando.Parameters["@capacidad"].Value = sala.Capacidad;
                    comando.Parameters["@disponible"].Value = (sala.Disponible ? 1 : 0);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertar salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void InsertaSala(Salas sala)
        {
            try
            {
                if (ExisteSala(sala))
                {
                    MessageBox.Show("Ya existe una sala con el mismo número.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {


                    SqliteCommand comando;
                    Conexion.Open();
                    comando = Conexion.CreateCommand();
                    comando.Parameters.Add("@id", SqliteType.Integer);
                    comando.Parameters.Add("@disponible", SqliteType.Integer);
                    comando.Parameters.Add("@capacidad", SqliteType.Integer);
                    comando.Parameters.Add("@numero", SqliteType.Text);
                    comando.CommandText = "INSERT INTO salas VALUES(@id, @numero, @capacidad, @disponible)";


                    comando.Parameters["@id"].Value = sala.Id;
                    comando.Parameters["@numero"].Value = sala.Numero;
                    comando.Parameters["@capacidad"].Value = sala.Capacidad;
                    comando.Parameters["@disponible"].Value = (sala.Disponible ? 1 : 0);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertar sala en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void InsertaSesiones(ObservableCollection<Sesion> sesiones)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();
                comando.CommandText = "DELETE FROM sesiones";
                comando.ExecuteNonQuery();
                comando.Parameters.Add("@idSesion", SqliteType.Integer);
                comando.Parameters.Add("@pelicula", SqliteType.Integer);
                comando.Parameters.Add("@sala", SqliteType.Integer);
                comando.Parameters.Add("@hora", SqliteType.Text);
                comando.CommandText = "INSERT INTO sesiones VALUES(@idSesion, @pelicula, @sala, @hora)";
                foreach (Sesion sesion in sesiones)
                {
                    if (EsPosibleInsertarActualizarSesion(sesion))
                    {

                        comando.Parameters["@idSesion"].Value = sesion.Id;
                        comando.Parameters["@pelicula"].Value = sesion.Pelicula.Id;
                        comando.Parameters["@sala"].Value = sesion.Sala.Id;
                        comando.Parameters["@hora"].Value = sesion.Hora;
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertar sesiones en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        public void InsertaSesion(Sesion sesion)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();
                comando.Parameters.Add("@idSesion", SqliteType.Integer);
                comando.Parameters.Add("@pelicula", SqliteType.Integer);
                comando.Parameters.Add("@sala", SqliteType.Integer);
                comando.Parameters.Add("@hora", SqliteType.Text);
                comando.CommandText = "INSERT INTO sesiones VALUES(@idSesion, @pelicula, @sala, @hora)";

                if (EsPosibleInsertarActualizarSesion(sesion))
                {

                    comando.Parameters["@idSesion"].Value = sesion.Id;
                    comando.Parameters["@pelicula"].Value = sesion.Pelicula.Id;
                    comando.Parameters["@sala"].Value = sesion.Sala.Id;
                    comando.Parameters["@hora"].Value = sesion.Hora;
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertar sesiones en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        public void InsertaVenta(Ventas venta)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();
                comando.Parameters.Add("@idVenta", SqliteType.Integer);
                comando.Parameters.Add("@sesion", SqliteType.Integer);
                comando.Parameters.Add("@cantidad", SqliteType.Integer);
                comando.Parameters.Add("@pago", SqliteType.Text);
                comando.CommandText = "INSERT INTO ventas VALUES(@idVenta, @sesion, @cantidad, @pago)";



                comando.Parameters["@idVenta"].Value = venta.Id;
                comando.Parameters["@sesion"].Value = venta.Sesion.Id;
                comando.Parameters["@cantidad"].Value = venta.Cantidad;
                comando.Parameters["@pago"].Value = venta.Pago;
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertar venta en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool ExistenSalas()
        {
            bool existenSalas = false;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM salas";
                existenSalas = Convert.ToInt32(comando.ExecuteScalar()) != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Existen salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return existenSalas;
        }

        public bool ExisteSala(Salas sala)
        {
            bool existeSala = false;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM salas WHERE numero='" + sala.Numero + "' ";
                existeSala = Convert.ToInt32(comando.ExecuteScalar()) != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Existen salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return existeSala;
        }

        public bool ExistenSesiones()
        {
            bool existenSesiones = false;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM sesiones";
                existenSesiones = Convert.ToInt32(comando.ExecuteScalar()) != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Existen salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return existenSesiones;
        }

        public bool ExistenPeliculas()
        {
            bool existenSalas = false;
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM peliculas";
                existenSalas = Convert.ToInt32(comando.ExecuteScalar()) != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Existen salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return existenSalas;
        }

        public void ActualizaSala(Salas sala)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "UPDATE salas SET numero=@numero, capacidad=@capacidad, disponible=@disponible WHERE idSala=@idSala";
                comando.Parameters.Add("@idSala", SqliteType.Integer);
                comando.Parameters.Add("@numero", SqliteType.Text);
                comando.Parameters.Add("@capacidad", SqliteType.Integer);
                comando.Parameters.Add("@disponible", SqliteType.Integer);
                comando.Parameters["@idSala"].Value = sala.Id;
                comando.Parameters["@numero"].Value = sala.Numero;
                comando.Parameters["@capacidad"].Value = sala.Capacidad;
                comando.Parameters["@disponible"].Value = sala.Disponible ? 1 : 0;
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Actualizar salas en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void ActualizaSesion(Sesion sesion)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                //puede ser que se quiera actualizar a una sala que ya tenga 3 sesiones asociadas
                //o que la sala no esté disponible
                if (EsPosibleInsertarActualizarSesion(sesion))
                {
                    comando.CommandText = "UPDATE sesiones SET pelicula=@idPelicula, sala=@idSala, hora=@hora WHERE idSesion=@idSesion";
                    comando.Parameters.Add("@idSesion", SqliteType.Integer);
                    comando.Parameters.Add("@idPelicula", SqliteType.Integer);
                    comando.Parameters.Add("@idSala", SqliteType.Integer);
                    comando.Parameters.Add("@hora", SqliteType.Text);

                    comando.Parameters["@idSesion"].Value = sesion.Id;
                    comando.Parameters["@idPelicula"].Value = sesion.Pelicula.Id;
                    comando.Parameters["@idSala"].Value = sesion.Sala.Id;
                    comando.Parameters["@hora"].Value = sesion.Hora;
                    comando.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Actualizar sesion en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool EsPosibleInsertarActualizarSesion(Sesion sesion)
        {
            //No abro/cierro la conexión porque este método se llama antes de insertar una sesion y será ese método el que se encargue de abrir/cerrar la conexión
            bool esPosibleInsertar = false;
            try
            {
                SqliteCommand comando;
                comando = Conexion.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM sesiones WHERE sala=" + sesion.Sala.Id;
                esPosibleInsertar = Convert.ToInt32(comando.ExecuteScalar()) < 3;

                //compruebo si es posible insertar la sesión, porque si ya hay mas de tres
                //sesiones asicionadas a una misma sala no me hace falta saber si la sala esta disponible
                if (esPosibleInsertar)
                {
                    comando.CommandText = "SELECT disponible FROM salas WHERE idSala=" + sesion.Sala.Id;

                    //Ejecuto como un escalar porque sólo me va a devolver 0 ó distinto de cero
                    esPosibleInsertar = Convert.ToInt32(comando.ExecuteScalar()) != 0;

                    if (!esPosibleInsertar)
                        MessageBox.Show("La sala no está disponible.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    MessageBox.Show("Esta sala ya tiene asociada 3 sesiones.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es posible insertar sesión en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return esPosibleInsertar;
        }

        public void EliminaSesion(Sesion sesion)
        {
            try
            {
                SqliteCommand comando;
                Conexion.Open();
                comando = Conexion.CreateCommand();

                comando.CommandText = "DELETE FROM sesiones WHERE idSesion=" + sesion.Id;
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eliminar sesion en la BD: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                try
                {
                    if (Conexion.State == ConnectionState.Open)//si la conexión está abierta la cierro
                        Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
