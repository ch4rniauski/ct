class Program
{
    static void Main()
    {
        HashTableWithRehashing hashTableWithRehashing = new HashTableWithRehashing();
        BinaryTreeHashTable binaryTreeHashTable = new BinaryTreeHashTable();

        string[] identifiers = File.ReadAllLines("file.txt");

        foreach (var identifier in identifiers)
        {
            hashTableWithRehashing.AddIdentifier(identifier);
            binaryTreeHashTable.AddIdentifier(identifier);
        }

        while (true)
        {
            Console.Write("Введите значение идентификатора, которое хотите найти: ");
            string? searchIdentifier = Console.ReadLine();

            hashTableWithRehashing.SearchIdentifier(searchIdentifier);
            binaryTreeHashTable.SearchIdentifier(searchIdentifier);

            Console.WriteLine("\nСреднее количество сравнений для метода рехеширования: " + hashTableWithRehashing.GetAverageComparisons());
            Console.WriteLine("Среднее количество сравнений для метода бинарного дерева: " + binaryTreeHashTable.GetAverageComparisons());
            Console.WriteLine("-------------------------------------------------------------------------");
        }
    }
}

class HashTableWithRehashing
{
    private Dictionary<int, List<string>> hashTable = new Dictionary<int, List<string>>();
    private int totalComparisons = 0;

    public void AddIdentifier(string identifier)
    {
        int hash = identifier.GetHashCode();
        if (!hashTable.ContainsKey(hash))
            hashTable[hash] = new List<string>();

        hashTable[hash].Add(identifier);
    }

    public void SearchIdentifier(string? identifier)
    {
        int? hash = (identifier ?? "").GetHashCode();

        if (hashTable.ContainsKey(hash ?? 0) && hashTable[hash ?? 0].Contains(identifier ?? ""))
            Console.WriteLine("\nИдентификатор найденный с помощью метода рехеширования: " + identifier);
        else
            Console.WriteLine("\nИдентификатор НЕ найденный с помощью метода рехеширования: " + identifier);

        totalComparisons++;
    }

    public double GetAverageComparisons()
    {
        int tempTotalComparisons = totalComparisons;
        totalComparisons = 0;
        return (double)tempTotalComparisons / hashTable.Count;
    }
}

class BinaryTreeHashTable
{
    private TreeNode? root;
    private int totalComparisons = 0;

    public void AddIdentifier(string identifier)
    {
        root = Insert(root, identifier);
    }

    private TreeNode Insert(TreeNode? root, string identifier)
    {
        if (root == null)
            return new TreeNode(identifier);

        if (string.Compare(identifier, root.Identifier) < 0)
        {
            totalComparisons++;
            root.Left = Insert(root.Left, identifier);
        }
        else
        {
            totalComparisons++;
            root.Right = Insert(root.Right, identifier);
        }

        return root;
    }

    public void SearchIdentifier(string? identifier)
    {
        if (Search(root, identifier))
            Console.WriteLine("Идентификатор найденный с помощью метода бинарного дерева: " + identifier);
        else
            Console.WriteLine("Идентификатор НЕ найденный с помощью метода бинарного дерева: " + identifier);
    }

    private bool Search(TreeNode? root, string? identifier)
    {
        if (root == null)
            return false;

        if (identifier == root.Identifier)
            return true;

        totalComparisons++;

        if (string.Compare(identifier, root.Identifier) < 0)
            return Search(root.Left, identifier);
        else
            return Search(root.Right, identifier);
    }

    public double GetAverageComparisons()
    {
        int tempTotalComparisons = totalComparisons;
        totalComparisons = 0;
        return (double)tempTotalComparisons / GetTotalNodes(root);
    }

    private int GetTotalNodes(TreeNode? root)
    {
        if (root == null)
            return 0;

        return 1 + GetTotalNodes(root.Left) + GetTotalNodes(root.Right);
    }
}

class TreeNode
{
    public string Identifier { get; set; }
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }

    public TreeNode(string identifier)
    {
        Identifier = identifier;
        Left = null;
        Right = null;
    }
}
