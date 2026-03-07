using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Slots")]
    public List<Slot> inventorySlots = new List<Slot>();

    [Header("Items disponibles")]
    public List<Item> availableItems = new List<Item>();

    [Header("Base de datos")]
    public string nombreDB = "MyDatabase.sqlite";
    private string rutaDB;

    [Header("Usuario actual")]
    private string usuarioActual;

    void Start()
    {
        rutaDB = Application.persistentDataPath + "/" + nombreDB;
        CrearTablaInventario();

        // Asignar índices automáticamente a los slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].slotIndex = i;
        }

        Debug.Log("Ruta BD: " + Application.persistentDataPath);

    }

    private void CrearTablaInventario()
    {
        string dbUri = "URI=file:" + rutaDB;

        using (var conexion = new SqliteConnection(dbUri))
        {
            conexion.Open();

            string consulta = @"CREATE TABLE IF NOT EXISTS Inventario (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                usuario TEXT NOT NULL,
                slotIndex INTEGER NOT NULL,
                itemID TEXT,
                UNIQUE(usuario, slotIndex))";

            using (var comando = new SqliteCommand(consulta, conexion))
            {
                comando.ExecuteNonQuery();
            }

            Debug.Log("Tabla Inventario creada correctamente");
        }
    }

    public void SetUsuarioActual(string usuario)
    {
        usuarioActual = usuario;
        CargarInventario();
    }

    public void GuardarInventario()
    {
        if (string.IsNullOrEmpty(usuarioActual))
        {
            Debug.LogWarning("No hay usuario logueado");
            return;
        }

        string dbUri = "URI=file:" + rutaDB;

        try
        {
            using (var conexion = new SqliteConnection(dbUri))
            {
                conexion.Open();

                // Primero borrar el inventario anterior del usuario
                string deleteQuery = "DELETE FROM Inventario WHERE usuario = @usuario";
                using (var deleteCmd = new SqliteCommand(deleteQuery, conexion))
                {
                    deleteCmd.Parameters.AddWithValue("@usuario", usuarioActual);
                    deleteCmd.ExecuteNonQuery();
                }

                // Guardar el inventario actual
                foreach (var slot in inventorySlots)
                {
                    Item item = slot.GetItem();
                    string itemID = item != null ? item.ItemID : null;

                    string insertQuery = @"INSERT INTO Inventario (usuario, slotIndex, itemID) 
                                          VALUES (@usuario, @slotIndex, @itemID)";

                    using (var insertCmd = new SqliteCommand(insertQuery, conexion))
                    {
                        insertCmd.Parameters.AddWithValue("@usuario", usuarioActual);
                        insertCmd.Parameters.AddWithValue("@slotIndex", slot.slotIndex);
                        insertCmd.Parameters.AddWithValue("@itemID", (object)itemID ?? DBNull.Value);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }

            Debug.Log("Inventario guardado correctamente para " + usuarioActual);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar inventario: " + e.Message);
        }
    }

    public void CargarInventario()
    {
        if (string.IsNullOrEmpty(usuarioActual))
        {
            Debug.LogWarning("No hay usuario logueado");
            return;
        }

        

        string dbUri = "URI=file:" + rutaDB;

        try
        {
            using (var conexion = new SqliteConnection(dbUri))
            {
                conexion.Open();

                string consulta = "SELECT slotIndex, itemID FROM Inventario WHERE usuario = @usuario";

                using (var comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@usuario", usuarioActual);

                    using (IDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int slotIndex = reader.GetInt32(0);
                            string itemID = reader.IsDBNull(1) ? null : reader.GetString(1);

                            if (slotIndex >= 0 && slotIndex < inventorySlots.Count && !string.IsNullOrEmpty(itemID))
                            {
                                Item item = BuscarItemPorID(itemID);
                                if (item != null)
                                {
                                    inventorySlots[slotIndex].SetItem(item);
                                }
                            }
                        }
                    }
                }
            }

            Debug.Log("Inventario cargado correctamente para " + usuarioActual);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar inventario: " + e.Message);
        }
    }

    private Item BuscarItemPorID(string itemID)
    {
        foreach (var item in availableItems)
        {
            if (item.ItemID == itemID)
            {
                return item;
            }
        }
        return null;
    }

    public void OnInventoryChanged()
    {
        // Guardar automáticamente cuando cambie el inventario
        GuardarInventario();
    }

    // Método para añadir un item a un slot vacío
    public bool AñadirItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.GetItem() == null)
            {
                slot.SetItem(item);
                OnInventoryChanged();
                return true;
            }
        }

        Debug.Log("Inventario lleno");
        return false;
    }
}