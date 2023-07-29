# NPCScheduler
An algorithm for Time-Based NPC's in 3D games. Created for use in the Unity Engine, however could easily be adapted for other workflows.
![](https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExMXQwcWJ1bXoydmh4bGh0M2dpdHlpcXBmbzl0ZDF0dG1icDdxZDh0YSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/DaqVg3JfzvwQXxDc2t/giphy.gif)

## How Does it Work?
The algorithm takes in a few variables, such as: current time, the time the pathing should start, the time the pathing should finish, as well as the points to follow.
The algorithm itself can be demonstrated using this piecewise function:

Let:
```
    P = path points (p_1, p_2, ..., p_n)
    T = start and end times (t_start, t_end)
    t = the specified time
    L = total line length
    n = total number of points in P
    d = L/n
    t' = (t - t_start) / (t_end - t_start)
    l = L * t'
```
We're looking for a point q in the path at time t. The formula is as follows:
```
For all i ∈ {0, 1, ..., n-2}:

    If d * (i + 1) <= l < d * (i + 2), then
        q = p_{i+1} + (p_{i+2} - p_{i+1}) * ((l - d * (i + 1)) / (d * (i + 2) - d * (i + 1)))
```
This is a piecewise function that defines the point q for all possible segments in the path. This function covers all possible values of l (and therefore t). It works by determining which segment the NPC is on at time t and then linearly interpolating between the endpoints of that segment to find the exact position of the NPC.

## How Can I Implement This Into My Own Project?
To implement this into a Unity project, you simply need to feed the code a changing time variable (which is referenced in ScheduledNPC.cs as GM.currentTime) as well as an NPCPath instance that contains 3-Dimensional points in your scene.
