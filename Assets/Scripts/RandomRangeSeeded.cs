using UnityEngine;

public class RandomRangeSeeded : MonoBehaviour
{
    static public int Generate(int low, int high)
    {
        System.Guid guid = System.Guid.NewGuid();
        int seed = GetSeedFromGuid(guid);
        Random.InitState(seed);
        return Random.Range(low, high);
    }
    static private int GetSeedFromGuid(System.Guid guid)
    {
        byte[] bytes = guid.ToByteArray();
        int seed = System.BitConverter.ToInt32(bytes, 0); // Use the first 4 bytes to get an int32
        return seed;
    }
}
