namespace Rdessoy_MCMS_Practical_Interview_Test.Data;

public class HashModel
{
    public int Id { get; set; }

    public string Hash { get; set; }

}

// No longer using the attributes as seen below as they're for the DBContext connection
//
//[Table("safe_hashes")]
//public class HashModel
//{
//    [Column("hash_id")]
//    public int Id { get; set; }

//    [Column("sha1")]
//    public string Hash { get; set; }

//}