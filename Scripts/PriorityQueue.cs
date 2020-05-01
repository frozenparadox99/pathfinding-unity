using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T> where T:IComparable<T>
{
    List<T> data;

    public int Count { get { return data.Count; } }

    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);

        int childindex = data.Count - 1;

        while(childindex > 0)
        {
            int parentidx = (childindex - 1) / 2;
            if (data[childindex].CompareTo(data[parentidx]) >= 0)
            {
                break;
            }

            T tmp = data[childindex];
            data[childindex] = data[parentidx];
            data[parentidx] = tmp;

            childindex = parentidx;
        }
    }

    public T Dequeue()
    {
        int lastindex = data.Count - 1;

        T frontItem = data[0];

        data[0] = data[lastindex];

        data.RemoveAt(lastindex);
        lastindex--;
        int parentIndex = 0;

        while (true)
        {
            int childidx = parentIndex * 2 + 1;
            if (childidx > lastindex)
            {
                break;
            }

            int rightChild = childidx + 1;

            if (rightChild <= lastindex && data[rightChild].CompareTo(data[childidx])<0)
            {
                childidx = rightChild;
            }

            if(data[parentIndex].CompareTo(data[childidx])<= 0)
            {
                break;
            }

            T tmp = data[parentIndex];
            data[parentIndex] = data[childidx];
            data[childidx] = tmp;

            parentIndex = childidx;
        }



        return frontItem;
    }

    public T peek()
    {
        T frontItem = data[0];
        return frontItem;
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public List<T> ToList()
    {
        return data;
    }

}
