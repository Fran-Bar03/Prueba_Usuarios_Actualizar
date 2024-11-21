using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace UsuariosActualizar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ruta al ChromeDriver
            string chromeDriverPath = @"C:\Users\Hachi\Downloads\chromedriver-win64";

            // Inicializar el WebDriver
            IWebDriver driver = new ChromeDriver(chromeDriverPath);

            try
            {
                // 1. Navegar al sitio de login
                driver.Navigate().GoToUrl("http://elitefitnesscenter.somee.com/Login.aspx");
                driver.Manage().Window.Maximize();

                // 2. Ingresar credenciales y hacer login
                driver.FindElement(By.Id("email")).SendKeys("alexagastelum05@gmail.com");
                driver.FindElement(By.Id("pass")).SendKeys("alexa");
                driver.FindElement(By.Id("btningresar")).Click();
                Thread.Sleep(2000);

                // 3. Navegar a la página de usuarios
                driver.Navigate().GoToUrl("http://elitefitnesscenter.somee.com/Usuarios.aspx");
                Thread.Sleep(2000);

                // 4. Buscar al usuario por correo en el GridView
                IWebElement table = driver.FindElement(By.Id("GridView_Usuarios"));
                var rows = table.FindElements(By.TagName("tr"));

                bool userFound = false;

                foreach (var row in rows)
                {
                    if (row.Text.Contains("mongee@gmail.com")) // Cambia por el correo que buscas
                    {
                        // Esperar hasta que el enlace "Select" sea clickeable
                        IWebElement selectButton = row.FindElement(By.LinkText("Select"));

                        // Usar JavaScript para hacer clic
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", selectButton);

                        userFound = true;
                        break;
                    }
                }

                if (!userFound)
                {
                    Console.WriteLine("El usuario especificado no fue encontrado.");
                    return;
                }

                // 5. Esperar a que los datos se carguen completamente en el formulario
                Thread.Sleep(2000); // Esperar por 2 segundos

                // 6. Modificar los campos permitidos
                string newPassword = "emiliano";
                string newCelular = "6442301589";
                string newPeso = "70";
                string newAltura = "170";
                string newTipo = "1"; // Administrador

                // Validación de campos vacíos
                if (string.IsNullOrWhiteSpace(newPassword) ||
                    string.IsNullOrWhiteSpace(newCelular) ||
                    string.IsNullOrWhiteSpace(newPeso) ||
                    string.IsNullOrWhiteSpace(newAltura) ||
                    string.IsNullOrWhiteSpace(newTipo))
                {
                    Console.WriteLine("Error: Todos los campos deben ser completados.");
                    return; // Detener el proceso si algún campo está vacío
                }

                // Llenar los campos con los valores nuevos
                driver.FindElement(By.Id("tbPassword")).Clear();
                driver.FindElement(By.Id("tbPassword")).SendKeys(newPassword);

                driver.FindElement(By.Id("tbCelular")).Clear();
                driver.FindElement(By.Id("tbCelular")).SendKeys(newCelular);

                driver.FindElement(By.Id("tbPeso")).Clear();
                driver.FindElement(By.Id("tbPeso")).SendKeys(newPeso);

                driver.FindElement(By.Id("tbAltura")).Clear();
                driver.FindElement(By.Id("tbAltura")).SendKeys(newAltura);

                driver.FindElement(By.Id("tbTipo")).Clear();
                driver.FindElement(By.Id("tbTipo")).SendKeys(newTipo);

                // 7. Hacer clic en el botón "Actualizar"
                IWebElement btnActualizar = driver.FindElement(By.Id("btnActualizar"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnActualizar);

                // Esperar para que el alert aparezca (5 segundos)
                Thread.Sleep(5000);

                // 8. Verificar y aceptar el alert
                try
                {
                    IAlert alert = driver.SwitchTo().Alert(); // Cambiar al alert
                    Console.WriteLine("Mensaje de alerta: " + alert.Text); // Mostrar el mensaje de la alerta
                    alert.Accept(); // Aceptar el alert (cerrarlo)
                    Console.WriteLine("Usuario actualizado correctamente.");
                }
                catch (NoAlertPresentException)
                {
                    Console.WriteLine("No se mostró ningún alert.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
            finally
            {
                // Cerrar el navegador
                driver.Close();
            }
            
        }
    }
}
