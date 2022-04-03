using MySql.Data.MySqlClient;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULP{

  	public class RepositorioInmueble : RepositorioBase{
      public RepositorioInmueble(IConfiguration configuration) : base(configuration)
      {
        
      }

  

    public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.Id, Direccion, Ambientes, Superficie, Tipo, Uso, Precio,Latitud, Longitud, PropietarioId," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					//command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
						              	Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Tipo=reader.GetString(4),
                            Uso=reader.GetString(5),
                            Precio=reader.GetInt32(6),
                            Latitud = reader.GetDecimal(7),
                            Longitud = reader.GetDecimal(8),
                            PropietarioId= reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
							}
						};
						res.Add(inmueble);
					}
					connection.Close();
				}
			}
			return res;
		}
      public int Alta(Inmueble inmueble)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmuebles (Direccion, Ambientes, Superficie, Tipo,Uso,Precio,Latitud, Longitud, PropietarioId) " +
					"VALUES (@direccion, @ambientes, @superficie, @tipo, @uso, @precio,@latitud, @longitud, @PropietarioId);" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new MySqlCommand(sql, connection))
				{
					//command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue($"@{nameof(inmueble.Direccion)}", inmueble.Direccion);
					command.Parameters.AddWithValue($"@{nameof(inmueble.Ambientes)}", inmueble.Ambientes);
					command.Parameters.AddWithValue($"@{nameof(inmueble.Superficie)}", inmueble.Superficie);
          command.Parameters.AddWithValue($"@{nameof(inmueble.Tipo)}", inmueble.Tipo);
        	command.Parameters.AddWithValue($"@{nameof(inmueble.Uso)}", inmueble.Uso);
        	command.Parameters.AddWithValue($"@{nameof(inmueble.Precio)}", inmueble.Precio);
          command.Parameters.AddWithValue($"@{nameof(inmueble.Latitud)}", inmueble.Latitud);
					command.Parameters.AddWithValue($"@{nameof(inmueble.Longitud)}", inmueble.Longitud);
        	command.Parameters.AddWithValue($"@{nameof(inmueble.PropietarioId)}", inmueble.PropietarioId);    	
				
          
            
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					inmueble.Id = res;
					connection.Close();
				}
			}
			return res;
		}

    	public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Inmuebles WHERE Id = {id}";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					// command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

    		public int Modificacion(Inmueble inmueble)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = "UPDATE Inmuebles SET " +
					"Direccion=@direccion, Ambientes=@ambientes, Tipo=@tipo, Uso=@uso, Precio=@precio, Superficie=@superficie, Latitud=@latitud, Longitud=@longitud, PropietarioId=@propietarioId " +
					"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
					command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
          command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
          command.Parameters.AddWithValue("@uso", inmueble.Uso);
          command.Parameters.AddWithValue("@precio", inmueble.Precio);
					command.Parameters.AddWithValue("@superficie", inmueble.Superficie);
					command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
					command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
					command.Parameters.AddWithValue("@propietarioId", inmueble.PropietarioId);
					command.Parameters.AddWithValue("@id", inmueble.Id);
					// command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}


  public Inmueble ObtenerPorId(int id)
		{
			Inmueble? inmueble = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = $"SELECT i.Id, Direccion, Ambientes, Superficie, Tipo,Uso,Precio,Latitud, Longitud, PropietarioId, p.Nombre, p.Apellido" +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id" +
                    $" WHERE i.Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    // command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    // command.CommandType = CommandType.Text;
                      command.Parameters.AddWithValue($"@{nameof(id)}", id);
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						inmueble = new Inmueble
						{
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Tipo= reader.GetString(4),
                            Uso= reader.GetString(5),
                            Precio= reader.GetInt32(6),
                            Latitud = reader.GetDecimal(7),
                            Longitud = reader.GetDecimal(8),
                            PropietarioId = reader.GetInt32(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
                            }
                        };
					}
					connection.Close();
				}
			}
			return inmueble;
        }

  }
}