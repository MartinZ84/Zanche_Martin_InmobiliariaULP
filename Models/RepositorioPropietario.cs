using MySql.Data.MySqlClient;

namespace Zanche_Martin_InmobiliariaULP.Models;

public class RepositorioPropietario{
  string ConnectionString="Server=localhost;User=root;Password=;Database=inmozanche;SslMode=none";
  public RepositorioPropietario()
  {
    
  }
  	public IList<Propietario> ObtenerTodos()
		{
			var res = new List<Propietario>();
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email" +
                    $" FROM Propietarios";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					// command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						var p = new Propietario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
						
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

    	public int Alta(Propietario p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"INSERT INTO Propietarios (Nombre, Apellido, Dni, Telefono, Email) " +
					$"VALUES (@{nameof(p.Nombre)}, @{nameof(p.Apellido)}, @{nameof(p.Dni)}, @{nameof(p.Telefono)}, @{nameof(p.Email)});" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					
					command.Parameters.AddWithValue($"@{nameof(p.Nombre)}", p.Nombre);
					command.Parameters.AddWithValue($"@{nameof(p.Apellido)}", p.Apellido);
					command.Parameters.AddWithValue($"@{nameof(p.Dni)}", p.Dni);
					command.Parameters.AddWithValue($"@{nameof(p.Telefono)}", p.Telefono);
					command.Parameters.AddWithValue($"@{nameof(p.Email)}", p.Email);
					
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
				}
			}
			return res;
		}

    	public int Modificacion(Propietario p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"UPDATE Propietarios SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email " +
					$"WHERE Id = @id";        
     
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue($"@{nameof(p.Id)}", p.Id);
					command.Parameters.AddWithValue($"@{nameof(p.Nombre)}", p.Nombre);
					command.Parameters.AddWithValue($"@{nameof(p.Apellido)}", p.Apellido);
					command.Parameters.AddWithValue($"@{nameof(p.Dni)}", p.Dni);
					command.Parameters.AddWithValue($"@{nameof(p.Telefono)}", p.Telefono);
					command.Parameters.AddWithValue($"@{nameof(p.Email)}", p.Email);
					
					connection.Open();
					res = command.ExecuteNonQuery();
          connection.Close();
				}
			}
			return res;
		}

    
  	public Propietario ObtenerPorId(int id)
		{
			Propietario  ? p = null;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email" +
                    $" FROM Propietarios" + $" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					// command.CommandType = CommandType.Text;
          	command.Parameters.AddWithValue($"@{nameof(id)}", id);
					connection.Open();
					var reader = command.ExecuteReader();
					if  (reader.Read())
					{
						 p = new Propietario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
						};						
					}
					connection.Close();
				}
			}
			return p;
		}

public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"DELETE FROM Propietarios WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					// command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}