public static class Utility{
    public static bool CountdownTimer(ref float current_timer, float max_timer, float count){
        if(current_timer <= 0f){
            current_timer = max_timer;
            return true;
        }

        current_timer -= count;
        return false;
    }
}