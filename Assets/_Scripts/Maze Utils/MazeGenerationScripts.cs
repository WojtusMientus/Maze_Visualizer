using System.Collections.Generic;

public class MazeGenerationScripts
{
    
    private List<MazeGenerationBase> mazeCreatingAlgorithms;
    
    public MazeGenerationScripts()
    {
        mazeCreatingAlgorithms = new List<MazeGenerationBase>{new RandomizedDFS(), 
                                                              new SimplifiedPrims(),
                                                              new HuntAndKill()
        };
    }
    
    public MazeGenerationBase GetAlgoAtIndex(int index) => mazeCreatingAlgorithms[index];
    
}
