using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip actionSFX;

    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    [SerializeField] private Node _nodePrefab;

    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private List<BlockType> _types;
    [SerializeField] private float _travelTime = 0.2f;
    [SerializeField] private int _winCondition = 64;

    [SerializeField] private GameObject _winScreen,_loseScreen;

    private List<Node> _nodes;
    private List<Block> _blocks;
    private GameState _state;
    private int _round;

    private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);

    public System.Action OnWinEvent;
    public System.Action OnLoseEvent;

    void Start(){
      ChangeState(GameState.GenerateLevel);
    }

    private void ChangeState(GameState newState) {
      _state = newState;

      switch (newState) {
        case GameState.GenerateLevel:
          GenerateGrid();
          break;
        case GameState.SpawningBlocks:
          SpawnBlocks(_round++ == 0 ? 2 : 1);
          break;
        case GameState.WaitingInput:
          break;
        case GameState.Moving:
          break;
        case GameState.Win:
          _winScreen.SetActive(true);
          OnWinEvent?.Invoke();
          break;
        case GameState.Lose:
          _loseScreen.SetActive(true);
          OnLoseEvent?.Invoke();
          break;

        default:
          throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
      }
    }

    void Update() {
      if(_state != GameState.WaitingInput) {
        return;
      }

      if(Input.GetKeyDown(KeyCode.LeftArrow)) {
        Shift(Vector2.left);
      }

      if(Input.GetKeyDown(KeyCode.RightArrow)) {
        Shift(Vector2.right);
      }

      if(Input.GetKeyDown(KeyCode.UpArrow)) {
        Shift(Vector2.up);
      }

      if(Input.GetKeyDown(KeyCode.DownArrow)) {
        Shift(Vector2.down);
      }
    }


    void GenerateGrid() {
      _round = 0;
      _nodes = new List<Node>();
      _blocks = new List<Block>();
      for (int x = 0; x < _width; x++) {
        for(int y = 0; y < _height; y++) {
          var node = Instantiate(_nodePrefab, new Vector3(x,y,0) + transform.position, Quaternion.identity,transform);
          _nodes.Add(node);
        }
      }

      var center = new Vector2((float) _width /2 - 0.5f,(float) _height / 2 -0.5f);
      var board =  Instantiate(_boardPrefab, (Vector3)center + transform.position, Quaternion.identity, transform);
      board.size = new Vector2(_width, _height);

      Camera.main.transform.position = new Vector3(center.x, center.y,-10);

      ChangeState(GameState.SpawningBlocks);
    }


    void SpawnBlocks(int amount) {

      var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();

      foreach (var node in freeNodes.Take(amount)) {
        SpawnBlock(node, 2);
      }

      if(freeNodes.Count() == 1) {
        //lost the GameManager
        ChangeState(GameState.Lose);
        return;
      }

      ChangeState(_blocks.Any(b=>b.Value == _winCondition) ? GameState.Win : GameState.WaitingInput);


    }

    void SpawnBlock(Node node, int value) {
      var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity, transform);
      block.Init(GetBlockTypeByValue(value));
      block.SetBlock(node);
      _blocks.Add(block);
    }

    void Shift(Vector2 dir) {
        source.PlayOneShot(actionSFX);

      ChangeState(GameState.Moving);

      var orderedBlocks = _blocks.OrderBy(b=>b.Pos.x).ThenBy(b=>b.Pos.y).ToList();

      if(dir == Vector2.right || dir ==  Vector2.up) {
        orderedBlocks.Reverse();
      }

      foreach (var block in orderedBlocks) {
        var next = block.Node;
        do {
            block.SetBlock(next);

            var possibleNode = GetNodeAtPosition(next.Pos + dir); //could return null if out of bounds
            if(possibleNode != null) {
              //know the node is available
              // possible to merge, set merge
              if(possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Value)) {
                block.MergeBlock(possibleNode.OccupiedBlock);

              }
              // otherwise can we move to this spot?
              else if(possibleNode.OccupiedBlock == null) {
                next = possibleNode;
              }
            }

        } while (next != block.Node);




      }

      var sequence = DOTween.Sequence();

      foreach(var block in orderedBlocks) {
        var movePoint = block.MergingBlock != null ? block.MergingBlock.Node.Pos : block.Node.Pos;
        sequence.Insert(0, block.transform.DOMove(movePoint, _travelTime).SetEase(Ease.InQuad));
      }

      sequence.OnComplete(() => {
        var mergeBlocks = orderedBlocks.Where(b=> b.MergingBlock !=null).ToList();
        foreach(var block in orderedBlocks.Where(b=>b.MergingBlock != null)) {
          MergeBlocks(block.MergingBlock, block);
        }

        ChangeState(GameState.SpawningBlocks);
      });



    }

    void MergeBlocks(Block baseBlock, Block mergingBlock) {
      var newValue = baseBlock.Value * 2;

      //Instantiate()
      SpawnBlock(baseBlock.Node, baseBlock.Value * 2);

      RemoveBlock(baseBlock);
      RemoveBlock(mergingBlock);
    }

    void RemoveBlock(Block block) {
      _blocks.Remove(block);
      Destroy(block.gameObject);
    }

    Node GetNodeAtPosition(Vector2 pos) {
      return _nodes.FirstOrDefault(n=> n.Pos == pos);
    }

} //end class

[Serializable]
public struct BlockType {
  public int Value;
//  public Color color;
  public Sprite sprite;

}


public enum GameState {
  GenerateLevel,
  SpawningBlocks,
  WaitingInput,
  Moving,
  Win,
  Lose
}
