using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ConfigApi;

namespace config_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController(ILogger<ConfigurationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets deserialized value from the database based on a given key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Deserialized value or null.</returns>
        /// <exception cref="Exception">Failed to query the database.</exception>
        [HttpGet("GetValue/{key}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public object? GetValue([Required]string key)
        {
            try
            {
                return DatabaseUtility.GetValueByKey(key);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets an array of deserialized values from the database based on a given value keyword.
        /// </summary>
        /// <param name="keyword">Value keyword</param>
        /// <returns>Array of deserialized values.</returns>
        /// <exception cref="Exception">Error connecting to the database or reading database values.</exception>
        [HttpGet("GetValues/{keyword}")]
        [ProducesResponseType(typeof(object[]), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public object[] GetValues(string keyword)
        {
            try
            {
                return DatabaseUtility.GetValuesByKeyword(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new key and value to the database if it does not exist.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>Deserialized value or null.</returns>
        /// <exception cref="Exception">The given key already exists in the database.</exception>
        [HttpPost("AddValue")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public bool AddValue([Required] string key, [Required] object value)
        {
            if (DatabaseUtility.KeyExists(key))
            {
                throw new Exception(string.Format("The key '{0}' already exists!", key));
            }
            DatabaseUtility.InsertKeyValue(key, value);
            return true;
        }

        /// <summary>
        /// Updates an existing key's value to a new given value.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if successful.</returns>
        /// <exception cref="Exception">The given key does not exist in the database.</exception>
        [HttpPut("UpdateValue")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public bool UpdateValue([Required]string key, [Required]object value)
        {
            if (DatabaseUtility.KeyExists(key))
            {
                DatabaseUtility.InsertKeyValue(key, value);
                return true;
            }
            else
            {
                throw new Exception(string.Format("The key '{0}' does not exist!", key));
            }
        }

        /// <summary>
        /// Deletes a given key from the database if it exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if successful.</returns>
        /// <exception cref="Exception">The given key does not exist in the database.</exception>
        [HttpDelete("DeleteValue")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public bool DeleteValue([Required]string key)
        {
            string command = @"
                                DELETE FROM storage
                                WHERE key = '{0}';
                            ";
            if (!DatabaseUtility.KeyExists(key))
            {
                throw new Exception(string.Format("The key '{0}' does not exist!", key));
            }
            string commandText = string.Format(command, key);
            DatabaseUtility.ExecuteCommand(commandText);
            return true;
        }
    }
}