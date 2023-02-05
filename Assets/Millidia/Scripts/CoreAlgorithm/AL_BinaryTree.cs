
    using System.Collections.Generic;
    //声明类：二叉树
    public class AL_BinaryTree<T>
    {
        //声明字段：根节点；
        public Node<T> head;
        //声明字段：用于存储遍历列表
        public List<T> preOrderList = new List<T>();
        public List<T> midOrderList = new List<T>();
        public List<T> postOrderList = new List<T>();
        //声明属性：
        public Node<T> Head { get { return head; } set { head = value; } }
 
        //重载构造函数：
        public AL_BinaryTree(T data)
        {
            Node<T> p = new Node<T>(data);
            head = p;
        }
 
        public AL_BinaryTree()
        {
            head = null;
        }
        /// <summary>
        /// //声明方法：判断是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return head == null;
        }
        /// <summary>
        ///  //声明方法：使用递归进行前序遍历
        /// </summary>
        /// <param name="root"></param>//  
        public void PreOrder(Node<T> root)
        {
            if (IsEmpty())
            {
                //Debug.
            }
 
            if (root != null)
            {
                preOrderList.Add(root.data);//将结果保存在列表中；
                PreOrder(root.LChild);
                PreOrder(root.rChild);
            }
        }
        /// <summary>
        /// ///声明方法：使用递归进行中序遍历
        /// </summary>
        /// <param name="root"></param>
        public void MidOrder(Node<T> root)
        {
            if (IsEmpty())
            {
                //Console.WriteLine("Tree is empty!!");
            }
 
            if (root != null)
            {
                //在这里体现左、根、右的遍历顺序：
                MidOrder(root.LChild);
                midOrderList.Add(root.data);//将结果保存在列表中；
                MidOrder(root.rChild);
            }
        }
        /// <summary>
        ///  //声明方法：使用递归进行后序遍历
        /// </summary>
        /// <param name="root"></param>
        public void PostOrder(Node<T> root)
        {
            if (IsEmpty())
            {
               // Console.WriteLine("Tree is empty!!");
            }
 
            if (root != null)
            {
                //在这里体现根、左、右的遍历顺序：
                PostOrder(root.LChild);
                PostOrder(root.rChild);
                postOrderList.Add(root.data);//将结果保存在列表中；
            }
        }
    }

    public class Node<T>
    {
        //声明字段：数据域、左孩子、右孩子；
        public T data;
        public Node<T> lChild;
        public Node<T> rChild;
 
        //声明属性：
        public T Data { get => data; set => data = value; }
        public Node<T> LChild { get => lChild; set => lChild = value; }
        public Node<T> RChild { get => rChild; set => rChild = value; }
 
        //重载构造函数：
        public Node(T data)
        {
            this.data = data;
        }
 
        public Node(T data, Node<T> lChild, Node<T> rChild)
        {
            this.data = data;
            this.lChild = lChild;
            this.rChild = rChild;
        }
 
        public Node()
        {
            this.data = default;
            this.lChild = null;
            this.rChild = null;
        }
    }