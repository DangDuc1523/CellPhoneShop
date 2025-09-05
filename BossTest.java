public class BossTest {
    public static void main(String[] args) {
        System.out.println("=== Test Examples ===");
        
        // VD1: n=5, j=2, k=3, a=[3,2,4,4,1]
        int[] a1 = {3, 2, 4, 4, 1};
        boolean result1 = Boss.canWin(5, 2, 3, a1);
        System.out.println("VD1: " + (result1 ? "YES" : "NO") + " (Expected: YES)");
        debugExample(5, 2, 3, a1, "VD1");
        
        // VD2: n=5, j=4, k=1, a=[5,3,4,5,2]  
        int[] a2 = {5, 3, 4, 5, 2};
        boolean result2 = Boss.canWin(5, 4, 1, a2);
        System.out.println("VD2: " + (result2 ? "YES" : "NO") + " (Expected: YES)");
        debugExample(5, 4, 1, a2, "VD2");
        
        // VD3: n=6, j=1, k=1, a=[1,2,3,4,5,6]
        int[] a3 = {1, 2, 3, 4, 5, 6};
        boolean result3 = Boss.canWin(6, 1, 1, a3);
        System.out.println("VD3: " + (result3 ? "YES" : "NO") + " (Expected: NO)");
        debugExample(6, 1, 1, a3, "VD3");
    }
    
    static void debugExample(int n, int j, int k, int[] a, String name) {
        System.out.println("\n=== Debug " + name + " ===");
        System.out.print("Original array: ");
        for (int x : a) System.out.print(x + " ");
        System.out.println();
        System.out.println("Player j=" + j + " has strength: " + a[j-1]);
        System.out.println("Need k=" + k + " winners");
        System.out.println();
    }
} 