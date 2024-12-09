namespace AdventOfCode24;

public class Day9
{
    private readonly record struct UsedSpace(string Id, int StartIndex, int Length);
    private readonly record struct FreeSpace(int StartIndex, int Length);
    private List<string?> _extendedMemory = [];
    private List<UsedSpace> _usedMemory = [];
    private List<FreeSpace> _freeMemory = [];

    private void Parse(string filename)
    {
        string text = File.ReadAllText(filename).Trim();
        
        bool isFile = true;
        int currentId = 0;
        int currentIndex = 0;

        foreach (var c in text)
        {
            int numberOfBlocks = int.Parse(c.ToString());
            IEnumerable<string?> blocks = Enumerable.Repeat(isFile ? currentId.ToString() : null, numberOfBlocks);
            _extendedMemory.AddRange(blocks);
            if (isFile)
            {
                _usedMemory.Add(new UsedSpace(currentId.ToString(), currentIndex, numberOfBlocks));
                currentId++;
            }
            else
            {
                _freeMemory.Add(new FreeSpace(currentIndex, numberOfBlocks));
            }
            
            currentIndex += numberOfBlocks;
            isFile = !isFile;
        }
    }

    // Part one
    private void Compress()
    {
        for (int i = _extendedMemory.Count - 1; i >= 0; i--)
        {
            if (_extendedMemory[i] is null) continue;
            
            int firstFreeSpace = _extendedMemory.IndexOf(null);
            if (firstFreeSpace >= i) break;
            
            _extendedMemory[firstFreeSpace] = _extendedMemory[i];
            _extendedMemory[i] = null;
        }
    }
    
    // Part two
    private void CompressWholeFiles()
    {
        for (int i = _usedMemory.Count - 1; i >= 0; i--)
        {
            var space = _freeMemory.FirstOrDefault(fm => fm.Length >= _usedMemory[i].Length);
            if (space == default)  continue;
            var spaceIndex = _freeMemory.IndexOf(space);

            UsedSpace previous = _usedMemory[i];
            if (space.StartIndex >= previous.StartIndex) continue; // otherwise we are moving it to the right
            
            _usedMemory[i] = previous with { StartIndex = space.StartIndex };
            
            _freeMemory[spaceIndex] = new FreeSpace(StartIndex: space.StartIndex + previous.Length, Length: space.Length - previous.Length);
            if (_freeMemory[spaceIndex].Length == 0) { _freeMemory.RemoveAt(spaceIndex); }
        }
    }

    private long Checksum()
    {
        long checksumTotal = 0;
        for (int i = 0; i < _extendedMemory.Count; i++)
        {
            if (_extendedMemory[i] is null) continue;
            int checksum = int.Parse(_extendedMemory[i]!) * i;
            checksumTotal += checksum;
        }
        return checksumTotal;
    }

    private long ChecksumWholeFiles()
    {
        long checksumTotal = 0;
        foreach (var usedSpace in _usedMemory)
        {
            int id = int.Parse(usedSpace.Id);
            for (int i = 0; i < usedSpace.Length; i++)
            {
                checksumTotal += (usedSpace.StartIndex + i) * id;
            }
        }

        return checksumTotal;
    }

    public void Solve(string filename)
    {
        Parse(filename);
        //Compress();
        //long checksum = Checksum();
        CompressWholeFiles();
        long checksum = ChecksumWholeFiles();
        Console.WriteLine($"Checksum: {checksum}");
    }
    
}