using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Unickq.Sitecore.BeAutomation.Utils
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public IList<TreeNode<T>> Children { get; set; }

        public bool IsRoot => Parent == null;

        public bool IsLeaf => Children.Count == 0;

        public int Level
        {
            get
            {
                if (IsRoot) return 0;
                return Parent.Level + 1;
            }
        }


        public TreeNode(T data)
        {
            Data = data;
            Children = new List<TreeNode<T>>();

            ElementsIndex = new LinkedList<TreeNode<T>>();
            ElementsIndex.Add(this);
        }

        public TreeNode<T> AddChild(T child)
        {
            var childNode = new TreeNode<T>(child) {Parent = this};
            Children.Add(childNode);

            RegisterChildForSearch(childNode);

            return childNode;
        }

        public override string ToString()
        {
            return Data != null ? Data.ToString() : "[data null]";
        }


        #region searching

        private ICollection<TreeNode<T>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            Parent?.RegisterChildForSearch(node);
        }

        public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate);
        }

        #endregion


        #region iterating

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in Children)
            foreach (var anyChild in directChild)
                yield return anyChild;
        }

        #endregion
    }
}
