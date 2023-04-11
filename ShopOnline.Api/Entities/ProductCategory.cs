using MessagePack;

namespace ShopOnline.API.Entities
{
    public class ProductCategory
    {
        [Key("Id")]
        public int Id { get; set; }
        public string Name { get; set; }    
    }
}
