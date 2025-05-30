using System.Collections;
using System.Collections.Generic;
using GridSystem.Controllers;
using UnityEngine;

namespace GridSystem.Models
{
    public class GridObject<T>
    {
        /// <summary>
        /// EN: We made this class generic so that it would be more dynamic and scalable for us to put another object instead of a grass manager or building manager
        /// TR: Bu sınıfı gridler daha dinamik ve scalable olması için generic yaptım.
        /// </summary>
        private GridSystem2D<GridObject<T>> _grid;
        private int _x;
        private int _y;
        private T _manager;


        public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void SetValue(T manager)
        {
            _manager = manager;
        }

        public T GetValue()
        {
            return _manager;
        }

        public int GetX => _x;
        public int GetY => _y;
        public GridSystem2D<GridObject<T>> GetGridSystem => _grid;
    }
}
