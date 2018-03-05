namespace Assets.StoryTemplate.Infrastructure
{
    public static class An
    {
        /*
         * This class is created solely for semantic purposes to write a more readable code
         * So when you need a new Image in game you just write
         * var image = An.Image();
         */
        public static ImageBuilder Image()
        {
            return new ImageBuilder();
        }
    }
}
