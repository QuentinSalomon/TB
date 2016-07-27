using Concept.Model;

namespace Framework
{
    [IntlConceptName("Framework.Playlist.Name", "Playlist principal")]
    public class Playlist : ConceptComponent
    {
        #region Properties

        [ConceptViewVisible]
        [IntlConceptName("Framework.Playlist.Title", "Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    DoPropertyChanged(TitlePropertyName);
                }
            }
        }
        private string _title;
        public const string TitlePropertyName = "Title";

        [ConceptAutoCreate]
        [IntlConceptName("Framework.Playlist.Partitions", "Partitions")]
        public StaticListPartitionXylo Partitions { get; protected set; }

        #endregion

        public bool AddPartition(PartitionXylo partition)
        {
            bool existInPlaylist = false;
            foreach (PartitionXylo p in Partitions)
                if (p.Title == partition.Title)
                    existInPlaylist = true;
            if (!existInPlaylist)
                Partitions.Add(partition);
            return !existInPlaylist;
        }

        public void RemovePartitionAt(int index)
        {
            Partitions.RemoveAt(index);
        }

        public void RemovePartition(PartitionXylo partition)
        {
            Partitions.Remove(partition);
        }

        public void ClearPartitions()
        {
            Partitions.Clear();
        }
        
    }
}
