using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameModel;
using UnityEngine;
using ZergRush;

public class CoroutineEngine
{
    public CoroutineEngine()
    {
    }

    public CoroutineEngine(IEnumerator coro) : this()
    {
        if (coro != null)
            currentExecutionStack.Push(coro);
    }

    public static YieldFrame Yield = new YieldFrame();

    public class YieldFrame
    {
    }

    List<Func<IEnumerator>> queue = new List<Func<IEnumerator>>();
    Stack<IEnumerator> currentExecutionStack = new Stack<IEnumerator>();
    float currentDelay;
    public static YieldFrame SkipFrame = new YieldFrame();

    public bool active => currentExecutionStack.Count > 0 || queue.Count > 0;

    public void Update(float dt)
    {
        currentDelay -= dt;
        while (currentDelay <= 0)
        {
            if (currentExecutionStack.Count == 0)
            {
                if (queue.Count > 0)
                {
                    var coro = queue.TakeFirst()();
                    if (coro == null) continue;
                    currentExecutionStack.Push(coro);
                }
                else
                {
                    currentDelay = 0;
                    return;
                }
            }

            var animCoro = currentExecutionStack.Peek();
            if (animCoro.MoveNext())
            {
                var newAnim = animCoro.Current;
                if (newAnim == null)
                {
                    // to next frame
                }
                else if (newAnim is YieldFrame)
                {
                    // wait a frame
                    currentDelay = GameModel.FrameTime;
                }
                else if (newAnim is float delay)
                {
                    currentDelay += delay;
                }
                else if (newAnim is IEnumerator nestedCoro)
                {
                    currentExecutionStack.Push(nestedCoro);
                }
                else
                {
                    Debug.Log($"unknown type {newAnim} was yielded in BattleAnimation based coroutine");
                }
            }
            else
            {
                currentExecutionStack.Pop();
            }
        }
    }

    public void Queue(Func<IEnumerator> action)
    {
        queue.Add(action);
    }
}

public class ParallelExecution : IEnumerator
{
    public List<CoroutineEngine> engines = new List<CoroutineEngine>();
    public float dt => Time.fixedDeltaTime;

    public ParallelExecution()
    {
    }

    public ParallelExecution(IEnumerable<IEnumerator> coros)
    {
        engines.AddRange(coros.Select(c => new CoroutineEngine(c)));
    }

    public void Add(IEnumerator coro)
    {
        engines.Add(new CoroutineEngine(coro));
    }

    public void Reset() => throw new NotImplementedException();

    public bool MoveNext()
    {
        bool goingOn = false;
        foreach (var animationEngine in engines)
        {
            animationEngine.Update(dt);
            goingOn = goingOn || animationEngine.active;
        }

        return goingOn;
    }

    public object Current => CoroutineEngine.Yield;
}
