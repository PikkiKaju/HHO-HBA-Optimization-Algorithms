namespace MetaheuristicOptimizer.Services
{
    public class FileUploadService
    {
        public string UploadFunction(IFormFile file)
        {
            // extension
            List<string> validExtensions = new List<string> { ".dll" };
            string extension = Path.GetExtension(file.FileName);
            if (!validExtensions.Contains(extension))
            {
                return $"Extension is not valid ({string.Join(".", validExtensions)})";
            }
            // file size
            long size = file.Length;
            if (size > (1 * 1024 * 1024))
            {
                return "Maximum size can be 1mb";
            }
            // name changing
            string fileName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Functions");
            using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }
        public string UploadAlgorithm(IFormFile file)
        {
            // extension
            List<string> validExtensions = new List<string> { ".dll" };
            string extension = Path.GetExtension(file.FileName);
            if (!validExtensions.Contains(extension))
            {
                return $"Extension is not valid ({string.Join(".", validExtensions)})";
            }
            // file size
            long size = file.Length;
            if (size > (1 * 1024 * 1024))
            {
                return "Maximum size can be 1mb";
            }
            // name changing
            string fileName = Guid.NewGuid().ToString() + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Algorithms");
            using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }
    }
}
