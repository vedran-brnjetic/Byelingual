namespace Assets.StoryTemplate.Infrastructure
{
    public static class A
    /*
     * This class is created solely for semantic purposes to write a more readable code
     * So when you need a new Canvas in game you just write
     * var canvas = A.Canvas();
     */
    {
        public static CanvasBuilder Canvas()
        {
            return new CanvasBuilder();
        }

        public static ButtonBuilder Button()
        {
            return new ButtonBuilder();
        }

        public static GameObjectBuilder GameObject()
        {
            return new GameObjectBuilder();
        }

        public static SceneBuilder Scene()
        {
            return new SceneBuilder();
            
        }
    }
}
