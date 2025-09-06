namespace efcoreApi.Models.VM
{
    public class FileModel
    {
        public string imageid { get; set; }
        public string url { get; set; }
        public FileModel()
        {
            imageid = Guid.NewGuid().ToString();
        }
    }
}
