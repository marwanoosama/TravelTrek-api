from fastapi import FastAPI
from pydantic import BaseModel
from transformers import AutoTokenizer, AutoModelForTokenClassification, pipeline

app = FastAPI()

model_path = "hossamaladdin/ai-travel-planner-ner"
tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForTokenClassification.from_pretrained(model_path)
ner = pipeline("ner", model=model, tokenizer=tokenizer, aggregation_strategy="simple")

class Request(BaseModel):
    inputs: str

@app.post("/extract")
def extract(req: Request):
    results = ner(req.inputs)
    return [
        {
            "entity_group": r["entity_group"],
            "score": float(r["score"]),
            "word": r["word"],
            "start": int(r["start"]),
            "end": int(r["end"]),
        }
        for r in results
    ]