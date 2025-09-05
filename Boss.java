import java.util.*;

public class Boss {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Đọc input
        int n = scanner.nextInt(); // số người
        int j = scanner.nextInt(); // người thứ j (1-indexed)
        int k = scanner.nextInt(); // số người chiến thắng
        
        int[] a = new int[n];
        for (int i = 0; i < n; i++) {
            a[i] = scanner.nextInt();
        }
        
        // Giải quyết
        boolean result = canWin(n, j, k, a);
        System.out.println(result ? "YES" : "NO");
        
        scanner.close();
    }
    
    public static boolean canWin(int n, int j, int k, int[] a) {
        // Sức mạnh của người thứ j (chuyển từ 1-indexed sang 0-indexed)
        int playerJStrength = a[j - 1];
        
        // Đếm có bao nhiêu người có sức mạnh > j
        int strongerThanJ = 0;
        for (int i = 0; i < n; i++) {
            if (a[i] > playerJStrength) {
                strongerThanJ++;
            }
        }
        
        // j có cơ hội thắng nếu số người mạnh hơn j < k
        // Vì trong trường hợp tốt nhất, j có thể lọt vào k vị trí còn lại
        return strongerThanJ < k;
    }
    
    // Phương thức test với các ví dụ
    public static void testExamples() {
        System.out.println("=== Test Examples ===");
        
        // VD1: n=5, j=2, k=3, a=[3,2,4,4,1]
        // Người thứ 2 có sức mạnh = 2
        // Top 3 mạnh nhất: [4,4,3], người yếu nhất trong top 3 = 3
        // 2 < 3 => NO... Wait, đáp án là YES
        
        // VD1
        int[] a1 = {3, 2, 4, 4, 1};
        boolean result1 = canWin(5, 2, 3, a1);
        System.out.println("VD1: " + (result1 ? "YES" : "NO")); // Expected: YES
        
        // VD2
        int[] a2 = {5, 3, 4, 5, 2};
        boolean result2 = canWin(5, 4, 1, a2);
        System.out.println("VD2: " + (result2 ? "YES" : "NO")); // Expected: YES
        
        // VD3
        int[] a3 = {1, 2, 3, 4, 5, 6};
        boolean result3 = canWin(6, 1, 1, a3);
        System.out.println("VD3: " + (result3 ? "YES" : "NO")); // Expected: NO
    }
} 