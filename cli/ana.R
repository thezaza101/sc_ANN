library(tidyverse)
library(plotly)


data <- read_csv("abaloneOutNorm.csv")

qt <- quantile(data$MaxTest)

data <- data %>%
  mutate(sz=MaxTest) %>%
  mutate(sz=replace(sz, sz<qt[1], 1)) %>%
  mutate(sz=replace(sz, sz>=qt[2]&sz<qt[5], 3)) %>%
  mutate(sz=replace(sz, sz>=qt[5], 6)) 
  

p <- plot_ly(data, x = ~Epochs, y = ~HiddenLayers, z = ~eta, color =~MeanTrainNorm, size=50) %>%
  add_markers() %>%
  layout(scene = list(xaxis = list(title = 'Epochs'),
                      yaxis = list(title = 'HiddenLayers'),
                      zaxis = list(title = 'eta')))

#p <- plot_ly(data, x = ~Epochs, y = ~HiddenLayers, z = ~eta, color =~MeanTrainNorm, size=~sz) %>%
#  add_markers() %>%
#  layout(scene = list(xaxis = list(title = 'Epochs'),
#                      yaxis = list(title = 'HiddenLayers'),
#                      zaxis = list(title = 'eta')))
