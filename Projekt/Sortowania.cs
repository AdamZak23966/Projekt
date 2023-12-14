using System.Collections.Generic;

public class SortAlgorithm
{
    public void BubbleSort(List<int> data)
    {
        int n = data.Count;
        bool swapped;
        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (data[j] > data[j + 1])
                {
                    // Zamiana miejscami
                    int tmp = data[j];
                    data[j] = data[j + 1];
                    data[j + 1] = tmp;
                    swapped = true;
                }
            }

            // Jeśli nie było zamiany w danej iteracji, to zakończ sortowanie
            if (!swapped)
                break;
        }
    }

    public void SelectionSort(List<int> data)
    {
        int n = data.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (data[j] < data[minIndex])
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                // Zamiana miejscami tylko jeśli znajdziemy nowy minimum
                int tmp = data[minIndex];
                data[minIndex] = data[i];
                data[i] = tmp;
            }
        }
    }

    public void InsertionSort(List<int> data)
    {
        int n = data.Count;
        for (int i = 1; i < n; i++)
        {
            int tmp = data[i];
            int j = i - 1;

            // Przesuwanie elementów większych od key
            while (j >= 0 && data[j] > tmp)
            {
                data[j + 1] = data[j];
                j = j - 1;
            }
            data[j + 1] = tmp;
        }
    }

    public void QuickSort(List<int> data)
    {
        QuickSort(data, 0, data.Count - 1);
    }

    private void QuickSort(List<int> data, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(data, low, high);

            QuickSort(data, low, pi);
            QuickSort(data, pi + 1, high);
        }
    }

    private int Partition(List<int> data, int low, int high)
    {
        int p = data[low];
        int i = low - 1;
        int j = high + 1;

        while (true)
        {
            do
            {
                i++;
            } while (data[i] < p);

            do
            {
                j--;
            } while (data[j] > p);

            if (i >= j)
                return j;

            // Zamiana miejscami
            int tmp = data[i];
            data[i] = data[j];
            data[j] = tmp;
        }
    }

    public List<int> MergeSort(List<int> data)
    {
        if (data.Count <= 1)
            return data;

        int middle = data.Count / 2;
        List<int> left = new List<int>(data.GetRange(0, middle));
        List<int> right = new List<int>(data.GetRange(middle, data.Count - middle));

        left = MergeSort(left);
        right = MergeSort(right);

        return Merge(left, right);
    }

    private List<int> Merge(List<int> left, List<int> right)
    {
        List<int> result = new List<int>();
        int i = 0, j = 0;

        while (i < left.Count && j < right.Count)
        {
            if (left[i] < right[j])
            {
                result.Add(left[i]);
                i++;
            }
            else
            {
                result.Add(right[j]);
                j++;
            }
        }

        while (i < left.Count)
        {
            result.Add(left[i]);
            i++;
        }

        while (j < right.Count)
        {
            result.Add(right[j]);
            j++;
        }

        return result;
    }
}