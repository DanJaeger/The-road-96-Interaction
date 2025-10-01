using System.Collections.Generic;
using Ink.Runtime;
using System.IO;

public class DialogueVariables
{
    Dictionary<string, Ink.Runtime.Object> _variables;
    private Story _currentStory; // Track story being listened to

    public DialogueVariables(string globalsFilePath)
    {
        string inkFileContents = File.ReadAllText(globalsFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
        Story globalVariablesStory = compiler.Compile();

        _variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            _variables.Add(name, value);
        }
    }

    public void StartListening(Story story)
    {
        // Stop listening previous story if needed
        if (_currentStory != null)
        {
            StopListening(_currentStory);
        }

        _currentStory = story;

        // Push global variables into the story
        VariableToStory(story);

        // Subscribe once
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        if (story != null)
        {
            story.variablesState.variableChangedEvent -= VariableChanged;
        }
    }

    void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (_variables.ContainsKey(name))
        {
            _variables[name] = value; // safer update
        }
    }

    void VariableToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
