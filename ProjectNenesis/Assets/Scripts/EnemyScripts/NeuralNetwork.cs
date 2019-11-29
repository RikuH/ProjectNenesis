
using System.Collections.Generic;
using System;
public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers; //layers
    private float[][] neurons; //neuron matrix
    private float[][][] weights; //weight matrix
    private float fitness; //fitness of the network

    //Initilizes and neural network with random weights
    public NeuralNetwork(int[] layers){

        //deep copy of layers of this network 
        this.layers = new int[layers.Length];

        for (int i = 0; i < layers.Length; i++){
            this.layers[i] = layers[i];
        }
    
        //Generates matrix
        InitNeurons();
        InitWeights();
    }

    //Deep copy constructor 
    public NeuralNetwork(NeuralNetwork copyNetwork){
        this.layers = new int[copyNetwork.layers.Length];

        for (int i = 0; i < copyNetwork.layers.Length; i++){
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }

    private void CopyWeights(float[][][] copyWeights){
        for (int i = 0; i < weights.Length; i++){
            for (int j = 0; j < weights[i].Length; j++){
                for (int k = 0; k < weights[i][j].Length; k++){
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    //Create the neuron matrix
    private void InitNeurons(){

        //Neuron Initilization
        List<float[]> neuronsList = new List<float[]>();

        //Run through all layers
        for (int i = 0; i < layers.Length; i++){

            //Add layer to neuron list
            neuronsList.Add(new float[layers[i]]); 
        }

        //Convert list to array
        neurons = neuronsList.ToArray(); 
    }

    //Create weights matrix
    private void InitWeights(){

        //Weights list which will later will converted into a weights 3D array
        List<float[][]> weightsList = new List<float[][]>(); 

        //Iterate over all neurons that have a weight connection
        for (int i = 1; i < layers.Length; i++){

            //Layer weight list for this current layer (will be converted to 2D array)
            List<float[]> layerWeightsList = new List<float[]>(); 

            int neuronsInPreviousLayer = layers[i - 1]; 

            //Iterate over all neurons in this current layer
            for (int j = 0; j < neurons[i].Length; j++){

                //Neruons weights
                float[] neuronWeights = new float[neuronsInPreviousLayer]; 

                //Iterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
                for (int k = 0; k < neuronsInPreviousLayer; k++){
                    
                    //Give random weights to neuron weights
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f,0.5f);
                }
                //Add neuron weights of this current layer to layer weights
                layerWeightsList.Add(neuronWeights); 
            }
            //Add this layers weights converted into 2D array into weights list
            weightsList.Add(layerWeightsList.ToArray()); 
        }
        //Convert to 3D array
        weights = weightsList.ToArray();
    }

    //Feed forward this neural network with a given input array
    public float[] FeedForward(float[] inputs){

        //Add inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++){
            neurons[0][i] = inputs[i];
        }

        //Iterate over all neurons and compute feedforward values 
        for (int i = 1; i < layers.Length; i++){
            for (int j = 0; j < neurons[i].Length; j++){
                float value = 0f;

                for (int k = 0; k < neurons[i-1].Length; k++){
                    
                    //Sum of all weights connections of this neuron weight their values in previous layer
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; 
                }
                //Hyperbolic tangent activation
                neurons[i][j] = (float)Math.Tanh(value); 
            }
        }
        //Return output layer
        return neurons[neurons.Length-1]; 
    }

    //Mutate neural network weights
    public void Mutate(){
        for (int i = 0; i < weights.Length; i++){
            for (int j = 0; j < weights[i].Length; j++){
                for (int k = 0; k < weights[i][j].Length; k++){
                    float weight = weights[i][j][k];

                    //Mutate weight value 
                    float randomNumber = UnityEngine.Random.Range(0f,100f);

                    //If 1
                    if (randomNumber <= 2f){ 
                        //Flip sign of weight
                        weight *= -1f;
                    }

                    //If 2
                    else if (randomNumber <= 4f){ 
                        //Pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }

                    //If 3
                    else if (randomNumber <= 6f){ 
                        //Randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }

                    //If 4
                    else if (randomNumber <= 8f){   
                        //Randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void AddFitness(float fit){
        fitness += fit;
    }

    public void SetFitness(float fit){
        fitness = fit;
    }

    public float GetFitness(){
        return fitness;
    }

    //Compare two neural networks and sort based on fitness
    public int CompareTo(NeuralNetwork last){
        if (last == null) return 1;

        if (fitness > last.fitness)
            return 1;

        else if (fitness < last.fitness)
            return -1;
            
        else
            return 0;
    }

}
