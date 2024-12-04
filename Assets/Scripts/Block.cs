/* Acts as a registry to store information about blocks. I did this to avoid the overhead
 * of the 'new' keuword every time I created a block to assign to a chunk. Instead the block is 
 * generated at compile time and the chunks can reference the static registry
 */

using System.Collections.Generic;
using System.Linq;

public class Block
{
    public static readonly Block Air = new Block(false, 0, "Air");
    public static readonly Block Dirt = new Block(true, 1, "Dirt");
    public static readonly Block Grass = new Block(true, 2, "Grass");

    public bool IsSolid { get; }
    public int Id { get; }
    public string Name { get; }

    private Block(bool isSolid, int id, string name)
    {
        IsSolid = isSolid;
        Id = id;
        Name = name;
    }

    private static readonly List<Block> AllBlocks = new List<Block> { Air, Dirt, Grass };

    public static Block GetBlockById(int id)
    {
        return AllBlocks.FirstOrDefault(block => block.Id == id) ?? Air; // Default to Air if not found
    }

    public static IEnumerable<Block> GetAllBlocks()
    {
        return AllBlocks;
    }

    public override string ToString()
    {
        return Name;
    }
}