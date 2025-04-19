namespace ToDoList.Log
{
    public static class Logs
    {


        public static void LogToFIle(string title, string texto)
        {
            var tituloArquivo = DateTime.Now.ToString("yyyyMMdd") + ".txt";

            StreamWriter swLog;

            if (File.Exists(tituloArquivo))
            {
                swLog = File.AppendText(tituloArquivo );
            }
            else
            {
                swLog = new StreamWriter(tituloArquivo);    
            }
            
            swLog.WriteLine($"Log: ");
            swLog.Write("Data : "  + 
                DateTime.Now.ToLongDateString() + " " + 
                DateTime.Now.ToLongTimeString());
            swLog.WriteLine();
            swLog.WriteLine("Titulo : " + title);   
            swLog.WriteLine("Messagem : " + texto);
            swLog.WriteLine("--------------------------------------------------");
            swLog.WriteLine("");
            swLog.Close();
        }
    }
}
