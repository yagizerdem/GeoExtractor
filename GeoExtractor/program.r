library(RGPR)
library(httr)
library(jsonlite)
library(base64enc)
library(uuid)
library(stringr)
library(grid)  # Load grid for displaying images
library(png)   # Load png for reading images


# Get file path from command-line arguments
# Get arguments from C# (file paths)
args <- commandArgs(trailingOnly = TRUE)

if (length(args) < 2) {
  stop("Error: Missing file path arguments")
}

gpr_file_path <- args[1]  # First argument from C#
json_output_path <- args[2] # JSON output file path from C#

x <- readGPR(dsn = gpr_file_path , verbose = FALSE)

# Generate a UUID
my_uuid <- UUIDgenerate()

png_filename = str_interp("gpr_radargram${my_uuid}.png")

#show img in r studio
sink(tempfile())
png(png_filename, width = 1200, height = 800, res = 150)
plot(x)
dev.off()
sink()



#convert img to base64 string
img_raw <- readBin(png_filename, what = "raw", n = file.info(png_filename)$size)

# Encode as Base64
img_base64 <- base64encode(img_raw)


data <- x@data
depth <- x@depth
coords <- coord(x)
traces <- x@traces
depth <- x@depth
pos <- x@pos
time0 <- x@time0
time <- x@time
fid <- x@fid
rec <- x@rec
trans <- x@trans
coordref <- x@coordref
dz <- x@dz
dx <- x@dx
antsep <- x@antsep
name <- x@name
name <- x@name
description <- x@description
filepath <- x@filepath
depthunit <- x@depthunit
posunit <- x@posunit
surveymode <- x@surveymode
date <- x@date
crs <- x@crs
vel <- x@vel
delineations <- x@delineations
hd <- x@hd


# Convert to a List for JSON Conversion
data_to_send <- list(
  data = apply(data, c(1, 2), as.character),  # Convert numeric matrix to character
  depth = as.character(depth),
  coords = as.data.frame(coords),
  traces = as.character(traces),
  pos = as.character(pos),
  time0 = as.character(time0),
  time = as.character(time),
  fid = as.character(fid),
  rec = as.data.frame(rec),
  trans = as.data.frame(trans),
  coordref = as.character(coordref),
  dz = as.character(dz),
  dx = as.character(dx),
  antsep = as.character(antsep),
  name = as.character(name),
  description = as.character(description),
  filepath = as.character(filepath),
  depthunit = as.character(depthunit),
  posunit = as.character(posunit),
  surveymode = as.character(surveymode),
  date = as.character(date),
  crs = as.character(crs),
  vel = as.data.frame(vel),
  delineations = as.data.frame(delineations),
  hd = as.data.frame(hd),
  img_base64 = img_base64
)

json_payload <- toJSON(data_to_send, pretty = TRUE)

writeLines(json_payload, stdout())
flush.console()