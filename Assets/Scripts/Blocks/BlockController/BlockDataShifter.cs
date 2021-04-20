//#define DEBUGGING

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlockManager/BlockDataShifter", fileName =  "BlockDataShifter")]
public class BlockDataShifter : ScriptableObject
{
    [SerializeField] private HexGrid hexGrid;
    
    private readonly Queue<byte> emptySpots = new Queue<byte>();
    
    public void ShiftBlocksDown() //O(n) shift down + no resizing
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            emptySpots.Clear();
#if DEBUGGING  
            Debug.Log($"Width {width}");
#endif
            for (int height = HexGrid.Height - 1; height >= 0; height--)
            {
                if (hexGrid.BlocksActive[width, height] == false) //if block not active, save hexGrid empty spot
                {
#if DEBUGGING  
                    Debug.Log($"Empty height {height}");
#endif
                    emptySpots.Enqueue((byte)height);
                }
                else  if (emptySpots.Count > 0 && hexGrid.BlocksActive[width, height]) //if current block is active but empty spot exists, swap
                {
                    byte emptySpot = emptySpots.Dequeue();
                     
                    hexGrid.BlocksActive[width, emptySpot] = true;
                    hexGrid.BlocksActive[width, height] = false;

                    SwapBlocks(); 

                    emptySpots.Enqueue((byte)height);
#if DEBUGGING  
                    Debug.Log($"Swap {height} into {emptySpot}");
#endif
                    #region Local Functions
                    
                    void SwapBlocks()
                    {
                        var emptyBlock = hexGrid.GetBlockAt(width, emptySpot);

                        PushValidBlockDown();
                        MoveEmptyBlockUp();

                        #region Local Functions
                        
                        void PushValidBlockDown()
                        {
                            Block validBlockAbove = hexGrid.GetBlockAt(width, (byte) height);
 
                            hexGrid.SetBlockAt(width, emptySpot, validBlockAbove);
                            validBlockAbove.Coord = new BlockCoord(width, emptySpot);
                            validBlockAbove.ChangeName(width, emptySpot);
                        }

                        void MoveEmptyBlockUp()
                        {
                            hexGrid.SetBlockAt(width, (byte)height, emptyBlock);
                            emptyBlock.Coord = new BlockCoord(width, (byte)height);
                            emptyBlock.ChangeName(width, (byte)height);
                        }
                        
                        #endregion
                    }
                    
                    #endregion
                }
            }
        }
    }
}