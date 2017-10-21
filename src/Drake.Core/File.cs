namespace Drake.Core
{
    public class File
    {
        public int FileId { get; set; }
        public string Path { get; set; }
        public int Commits { get; set; }

        public int RepositoryId { get; set; }
        public Repository Repository { get; set; }
    }
}