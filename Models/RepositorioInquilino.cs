using MySql.Data.MySqlClient;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULi.Models;

public class RepositorioInquilino{
  string ConnectionString="Server=localhost;User=root;Password=;Database=inmozanche;SslMode=none";
  public RepositorioInquilino()
  {
    
  }
  	public IList<Inquilino> ObtenerTodos()
		{
			var res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email, Lugar_trabajo,Dni_garante,Nombre_garante,Apellido_garante,Telefono_garante" +
                    $" FROM Inquilinos";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					// command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						var i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
              Lugar_Trabajo = reader.GetString(6),
              Dni_Garante = reader.GetString(7),
              Nombre_Garante = reader.GetString(8),
              Apellido_Garante = reader.GetString(9),
              Telefono_Garante = reader.GetString(10),
             
						
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

    	public int Alta(Inquilino i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"INSERT INTO Inquilinos (Nombre, Apellido, Dni, Telefono, Email, Lugar_Trabajo, Dni_Garante, Nombre_Garante, Apellido_Garante, Telefono_Garante) " +
					$"VALUES (@{nameof(i.Nombre)}, @{nameof(i.Apellido)}, @{nameof(i.Dni)}, @{nameof(i.Telefono)}, @{nameof(i.Email)}, @{nameof(i.Lugar_Trabajo)},@{nameof(i.Dni_Garante)}, @{nameof(i.Nombre_Garante)} ,@{nameof(i.Apellido_Garante)}, @{nameof(i.Telefono_Garante)} );" +
					"SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					
					command.Parameters.AddWithValue($"@{nameof(i.Nombre)}", i.Nombre);
					command.Parameters.AddWithValue($"@{nameof(i.Apellido)}", i.Apellido);
					command.Parameters.AddWithValue($"@{nameof(i.Dni)}", i.Dni);
					command.Parameters.AddWithValue($"@{nameof(i.Telefono)}", i.Telefono);
					command.Parameters.AddWithValue($"@{nameof(i.Email)}", i.Email);
          command.Parameters.AddWithValue($"@{nameof(i.Lugar_Trabajo)}",i.Lugar_Trabajo);
          command.Parameters.AddWithValue($"@{nameof(i.Dni_Garante)}", i.Dni_Garante);
          command.Parameters.AddWithValue($"@{nameof(i.Nombre_Garante)}",i.Nombre_Garante);
          command.Parameters.AddWithValue($"@{nameof(i.Apellido_Garante)}",i.Apellido_Garante);    
          command.Parameters.AddWithValue($"@{nameof(i.Telefono_Garante)}",i.Telefono_Garante);
				
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
				}
			}
			return res;
		}

    	public int Modificacion(Inquilino i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"UPDATE Inquilinos SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email, Lugar_Trabajo=@lugar_trabajo, Dni_Garante=@dni_garante, Nombre_Garante=@nombre_garante, Apellido_Garante=@apellido_garante, Telefono_Garante=@telefono_garante " +
					$"WHERE Id = @id";        
     
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue($"@{nameof(i.Id)}", i.Id);
					command.Parameters.AddWithValue($"@{nameof(i.Nombre)}", i.Nombre);
					command.Parameters.AddWithValue($"@{nameof(i.Apellido)}", i.Apellido);
					command.Parameters.AddWithValue($"@{nameof(i.Dni)}", i.Dni);
					command.Parameters.AddWithValue($"@{nameof(i.Telefono)}", i.Telefono);
					command.Parameters.AddWithValue($"@{nameof(i.Email)}", i.Email);
          command.Parameters.AddWithValue($"@{nameof(i.Lugar_Trabajo)}",i.Lugar_Trabajo);
          command.Parameters.AddWithValue($"@{nameof(i.Dni_Garante)}",i.Dni_Garante);
          command.Parameters.AddWithValue($"@{nameof(i.Nombre_Garante)}",i.Nombre_Garante);
          command.Parameters.AddWithValue($"@{nameof(i.Apellido_Garante)}",i.Apellido_Garante);
          command.Parameters.AddWithValue($"@{nameof(i.Telefono_Garante)}",i.Telefono_Garante);			
					
					connection.Open();
					res = command.ExecuteNonQuery();
          connection.Close();
				}
			}
			return res;
		}

    
  	public Inquilino ObtenerPorId(int id)
		{
			Inquilino  ? i = null;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email, Lugar_trabajo,Dni_garante,Nombre_garante,Apellido_garante,Telefono_garante " +
                    $" FROM Inquilinos" + $" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					// command.CommandType = CommandType.Text;
          command.Parameters.AddWithValue($"@{nameof(id)}", id);
					connection.Open();
					var reader = command.ExecuteReader();
					if  (reader.Read())
					{
						 i = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
              Lugar_Trabajo = reader.GetString(6),
              Dni_Garante = reader.GetString(7),
              Nombre_Garante = reader.GetString(8),
              Apellido_Garante = reader.GetString(9),
              Telefono_Garante = reader.GetString(10),
						};						
					}
					connection.Close();
				}
			}
			return i;
		}

public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(ConnectionString))
			{
				string sql = $"DELETE FROM Inquilinos WHERE Id = @id";
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